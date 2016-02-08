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

	public int maxHP{get{return Mathf.RoundToInt((BattleAttribute.MaxHP(this) + maxHPAdd) * (1 + maxHPMulti));}}
	public int maxHPAdd = 0;
	public float maxHPMulti = 0;

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
	public Damage damage;//伤害值
	public List<Command> availableCommands = new List<Command>();
	public List<Buff> buffList = new List<Buff>();
	
	public bool isGuarding = false;
	public bool isEvading = false;
	public bool isBuffFrozen = false;

	protected BattleObjectUIEvent UIEvent;
	protected ObjectData data;

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (BattleEvent.OnTimelineUpdate, OnTimelineUpdate);
		EventManager.Instance.RegisterEvent(BattleEvent.BattleObjectDied, OnBattleObjectDied);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnTimelineUpdate, OnTimelineUpdate);
		EventManager.Instance.UnRegisterEvent(BattleEvent.BattleObjectDied, OnBattleObjectDied);
	}

	/*更新时间轴*/
	protected void OnTimelineUpdate(MessageEventArgs args)
	{
		if(isBuffFrozen)
			timelinePosition += 0;
		else if(battleStatus == BattleStatus.Prepare)
			timelinePosition += BattleAttribute.Speed(this);
		else if(battleStatus == BattleStatus.Action)
			timelinePosition += commandToExecute.preExecutionSpeed;

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

	protected void OnBattleObjectDied(MessageEventArgs args)
	{
		BattleObject bo = args.GetMessage<BattleObject> ("Object");
		if(this == bo)
		{
			UIEvent.DestoryUI ();
		}
	}

	protected void OnReady()
	{
		timelinePosition = GlobalDataStructure.BATTLE_TIMELINE_READY;
		isGuarding = false;
		isEvading = false;
		battleStatus = BattleStatus.Ready;

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
		
		foreach(Buff buff in buffList)
		{
			if(buff.Tick() == 0)
			{
				buffList.Remove(buff);
			}		
		}

		availableCommands = Command.GetAvailableCommands(this);
	}

	protected abstract void SelectCommand();

	protected virtual void ExecuteCommand()
	{
		SkillHelper.CheckBuff (BuffTrigger.Action, this);
		//decide command
		commandToExecute.source = this;
		BattleManager.Instance.AddToCommandQueue (commandToExecute);
		//post process
		timelinePosition = -commandToExecute.postExecutionRecover * BattleAttribute.Speed(this);//后退距离 = 帧 * 步进
		battleStatus = BattleStatus.Prepare;
		commandToExecute = new CommandNone();
	}

	public void AddBuff(int id, int effectTurns)
	{
		BuffData data = DataManager.Instance.GetSkillDataSet().GetBuffData(id);
		Buff buff = Buff.CreateBuff(this, data, effectTurns);
		buffList.Add(buff);
	}

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

	public List<int> GetMagic()
	{
		return data.magicIDs;
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
}

