using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



public abstract class BattleObject : MonoBehaviour {
/*
 * 所有参战物体共有的数据与逻辑。
 * */
	private int _currentHP;
	public int currentHP
	{
		set
		{
			if(value > maxHP) value = maxHP;
			if(value < 0) value = 0;
			_currentHP = value;
			UIEvent.SetHPBar(_currentHP);
		}
		get
		{
			return _currentHP;
		}
	}

	private int _timelinePosition;
	public int timelinePosition
	{
		set
		{
			if(value > GlobalDataStructure.BATTLE_TIMELINE_MAX) value = GlobalDataStructure.BATTLE_TIMELINE_MAX;
			_timelinePosition = value;
			UIEvent.SetAvatarPositionX(value <= 0 ? 0 : value / 20);//max:500
		}
		get
		{
			return _timelinePosition;
		}
	}

	public int maxHP{get{return BattleAttribute.MaxHP(this);}}

	private float _maxHPMulti;
	public float maxHPMulti
	{
		get{return _maxHPMulti;}
		set{
			float previousMulti = 1 + _maxHPMulti;
			_maxHPMulti = value;
			currentHP = Mathf.RoundToInt(currentHP * (1 + _maxHPMulti) / previousMulti);
			UIEvent.SetHPBar(currentHP, maxHP);
		}
	}
	public float speedMulti = 0;
	public float attackMulti = 0;
	public float defenceMulti = 0;
	public float accuracyMulti = 0;
	public float evasionMulti = 0;
	public float critMulti = 0;
	public float critResistMulti = 0;

	public int stamina{get{return data.stamina;}}
	public int power{get{return data.power;}}
	public int agility{get{return data.agility;}}
	public int skill{get{return data.skill;}}
	public int toughness{get{return data.toughness;}}
	public int insight{get{return data.insight;}}
	public int eloquence{get{return data.eloquence;}}
	public int luck{get{return data.luck;}}

	public BattleStatus battleStatus = BattleStatus.Prepare;
	public Command commandToExecute = new CommandNone();
	public Damage damage;//击出的伤害
	public Damage damageTaken;//受到的伤害
	public List<Command> availableCommands = new List<Command>();//可用指令
	public List<Buff> buffList = new List<Buff>();//激活buff

	public bool isDead = false;//是否已经死了
	public bool isBlocking = false;//是否正在格挡
	public bool isEvading = false;//是否正在躲避
	public bool isEnemy = false;//是否敌人
	public bool isHealingBottleUsed = false;//是否使用过元素瓶
	public bool isRecovering{get{return timelinePosition < 0;}}
	public float buffFrozenTime = 0;//冰冻BUFF剩余时间
	public BattleObject guardTarget = null;//守护对象
	public BattleObject guardedTarget = null;//被守护对象

	public bool disableAttackCommand;
	public bool disableDefenceCommand;
	public bool disableItemCommand;
	public bool disableStrategyCommand;

	protected BattleObjectUIEvent UIEvent;
	protected ObjectData data;

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (BattleEvent.OnTimelineUpdate, OnTimelineUpdate);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnTimelineUpdate, OnTimelineUpdate);
	}

	protected abstract void SelectCommand();
	public abstract bool IsBoss();

	/*-----时间轴相关-----*/

	protected void OnTimelineUpdate(MessageEventArgs args)
	{
		if(this.isDead)return;

		if(buffFrozenTime > 0)
			buffFrozenTime -= Time.deltaTime;
		else if(battleStatus == BattleStatus.Prepare)
			timelinePosition += BattleAttribute.Speed(this);
		else if(battleStatus == BattleStatus.Action)
			timelinePosition += commandToExecute.preExecutionSpeed * 2 / 3;

		if(timelinePosition >= GlobalDataStructure.BATTLE_TIMELINE_READY && battleStatus == BattleStatus.Prepare)
		{
			OnReady();
			SelectCommand();
		}
		if(timelinePosition >= GlobalDataStructure.BATTLE_TIMELINE_MAX && battleStatus == BattleStatus.Action)
		{
			ExecuteCommand();
		}
	}

	protected void OnReady()
	{
		timelinePosition = GlobalDataStructure.BATTLE_TIMELINE_READY;
		isBlocking = false;
		isEvading = false;
		battleStatus = BattleStatus.Ready;
		if(guardedTarget != null)
		{
			guardedTarget.guardTarget = null;
			guardedTarget = null;
		}

		if(data.battleType == BattleType.Physical)
		{//自愈机制
			if(currentHP >= maxHP * GlobalDataStructure.HP_RECOVER_THRESHOLD && currentHP < maxHP)
			{
				int recoverAmount = currentHP;
				currentHP += (int) (maxHP * GlobalDataStructure.HP_RECOVER_AMOUNT);
				recoverAmount = currentHP - recoverAmount;

				MessageEventArgs args = new MessageEventArgs();
				args.AddMessage("Name", GetName());
				args.AddMessage("Amount", recoverAmount);
				EventManager.Instance.PostEvent (BattleEvent.OnHPAutoRecover, args);
			}
		}
		else if(data.battleType == BattleType.Magical)
		{//回复机制
			if( currentHP < maxHP)
			{
				int recoverAmount = currentHP;
				currentHP += (int) (maxHP * GlobalDataStructure.MP_RECOVER_AMOUNT);
				recoverAmount = currentHP - recoverAmount;
				
				MessageEventArgs args = new MessageEventArgs();
				args.AddMessage("Name", GetName());
				args.AddMessage("Amount", recoverAmount);
				EventManager.Instance.PostEvent (BattleEvent.OnMPAutoRecover, args);
			}
		}

		List<Buff> toRemoveList = new List<Buff>();
		foreach(Buff buff in buffList)
		{
			buff.Tick();
			if(buff.effectTurns == 0)
			{
				toRemoveList.Add(buff);
			}
		}
		foreach(Buff buff in toRemoveList)
		{
			buffList.Remove(buff);	
		}

		availableCommands = Command.GetAvailableCommands(this);
	}

	protected virtual void ExecuteCommand()
	{
		SkillHelper.CheckBuff (BuffTrigger.Action, this);
		//decide command
		BattleManager.Instance.AddToCommandQueue (commandToExecute);
	}

	public void PostExecute()
	{
		timelinePosition = -commandToExecute.postExecutionRecover * BattleAttribute.Speed(this);//后退距离 = 帧 * 步进
		battleStatus = BattleStatus.Prepare;
		commandToExecute = new CommandNone();
	}

	/*-----Buff相关-----*/

	public void AddBuff(int id, int effectTurns)
	{
		BuffData data = DataManager.Instance.GetSkillDataSet().GetBuffData(id);
		Buff duplicatedBuff = buffList.Find((Buff temp)=>{return temp.id == id;});
		if(duplicatedBuff != null)
		{
			duplicatedBuff.effectTurns = Mathf.Max(effectTurns, duplicatedBuff.effectTurns);
		}
		else
		{
			Buff buff = Buff.CreateBuff(this, data, effectTurns);
			buffList.Add(buff);
		}
	}

	public void RemoveBuff(int id)
	{
		foreach(Buff buff in buffList)
		{
			if(buff.id == id)
			{
				buff.Remove();
				buffList.Remove(buff);	
				break;
			}
		}
	}

	public void ClearBuff()
	{
		List<Buff> toRemoveList = new List<Buff>();
		foreach(Buff buff in buffList)
		{
			if(!buff.isBornBuff)
			{
				buff.Remove();
				toRemoveList.Add(buff);
			}
		}
		foreach(Buff buff in toRemoveList)
		{
			buffList.Remove(buff);	
		}
	}

	/*-----数据相关-----*/

	public string GetName()
	{
		return data.name;
	}

	public void SetWeapon(int weaponID)
	{
		data.weaponID = weaponID;
	}

	public int GetWeapon()
	{
		return data.weaponID;
	}

	public List<int> GetWeaponList()
	{
		return data.GetWeapons();
	}

	public List<int> GetMagicList()
	{
		return data.GetMagics();
	}

	public Dictionary<int, int> GetItemList()
	{
		Dictionary<int, int> returnVal = new Dictionary<int, int>();
		foreach(var item in data.GetItemDict())
		{
			ItemData itemData = DataManager.Instance.GetItemDataSet().GetItemData(item.Key);
			if(itemData.usedInBattle)
				returnVal.Add(item.Key, item.Value);
		}
		return returnVal;
	}

	public BattleType GetBattleType()
	{
		return data.battleType;
	}

	public int GetItemCount(int itemID)
	{
		return data.GetItemCount(itemID);
	}

	public void ConsumeItem(int itemID)
	{
		data.ConsumeItem(itemID);
	}
	
	public void AcquireItem(int itemID, int count)
	{
		data.AcquireItem(itemID, count);
	}

	public float GetSkillTypeMulti(SkillType skillType)
	{
		switch(skillType)
		{
		case SkillType.Blunt:
			return data.bluntMulti;
		case SkillType.Slash:
			return data.slashMulti;
		case SkillType.Thrust:
			return data.thrustMulti;
		case SkillType.Neutral:
			return data.neutralMulti;
		case SkillType.Yin:
			return data.yinMulti;
		case SkillType.Yang:
			return data.yangMulti;
		}
		return 1f;
	}
}

