using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum BattleStatus
{
	Prepare,//等待选择行动(0~8000)
	Ready,//选择行动中(8000)
	Action,//即将行动(8000~10000)
}

public abstract class BattleObject : MonoBehaviour {
/*
 * 所有参战物体共有的数据与逻辑。
 * */
	public int maxHP{get{return BattleFormula.GetMaxHP(this);}}
	public int maxHPInc = 0;
	public int maxHPMulti = 0;

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

	public int stamina{get{return (int)(data.stamina * (1 + staminaMulti) + staminaInc);}}
	public int staminaInc = 0;
	public float staminaMulti = 0;

	public int power{get{return (int)(data.power * (1 + powerMulti) + powerInc);}}
	public int powerInc = 0;
	public float powerMulti = 0;
	
	public int agility{get{return (int)(data.agility * (1 + agilityMulti) + agilityInc);}}
	public int agilityInc = 0;
	public float agilityMulti = 0;
	
	public int toughness{get{return (int)(data.toughness * (1 + toughnessMulti) + toughnessInc);}}
	public int toughnessInc = 0;
	public float toughnessMulti = 0;
	
	public int insight{get{return (int)(data.insight * (1 + insightMulti) + insightInc);}}
	public int insightInc = 0;
	public float insightMulti = 0;
	
	public int skill{get{return (int)(data.skill * (1 + skillMulti) + skillInc);}}
	public int skillInc = 0;
	public float skillMulti = 0;
	
	public int luck{get{return (int)(data.luck * (1 + luckMulti) + luckInc);}}
	public int luckInc = 0;
	public float luckMulti = 0;
	
	public int eloquence{get{return (int)(data.eloquence * (1 + eloquenceMulti) + eloquenceInc);}}
	public int eloquenceInc = 0;
	public float eloquenceMulti = 0;	

	public BattleStatus battleStatus = BattleStatus.Prepare;
	public Command commandToExecute = new CommandNone();
	public Damage damage = new Damage();//伤害值
	public List<Command> availableCommands = new List<Command>();
	public List<Buff> buffList = new List<Buff>();

	public bool isPaused = true;
	public bool isGuarding = false;
	public bool isEvading = false;
	public bool isDied = false;

	private int _timelinePosition;
	public int timelinePosition//max:10000
	{
		set
		{
			if(value > 10000) value = 10000;
			_timelinePosition = value;
			if(value < 0) value = 0;
			UIEvent.SetAvatarPositionX(value / 20, isPaused);//max:500
		}
		get
		{
			return _timelinePosition;
		}
	}

	protected BattleObjectUIEvent UIEvent;
	protected ObjectData data;

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (EventDefine.UpdateTimeline, OnUpdateTimeline);		
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (EventDefine.UpdateTimeline, OnUpdateTimeline);
	}

	/*更新时间轴*/
	protected void OnUpdateTimeline(MessageEventArgs args)
	{
		if(!isPaused && !isDied)
		{	
			if(battleStatus == BattleStatus.Prepare)
				timelinePosition += (int)(Mathf.Log10(data.agility) * 100);
			else if(battleStatus == BattleStatus.Action)
				timelinePosition += commandToExecute.preExecutionSpeed;
		}

		if(timelinePosition >= 8000 && battleStatus == BattleStatus.Prepare)
		{
			SelectCommand();
		}
		if(timelinePosition >= 10000 && battleStatus == BattleStatus.Action)
		{
			ExecuteCommand();
		}
	}

	protected virtual void SelectCommand()
	{
		timelinePosition = 8000;
		isGuarding = false;
		isEvading = false;
		battleStatus = BattleStatus.Ready;

		foreach(Buff buff in buffList)
		{
			if(buff.Tick() <= 0)
			{
				buffList.Remove(buff);
			}
			else
			{
				buff.OnReady();
			}				
		}
	}

	protected virtual void ExecuteCommand()
	{
		foreach(Buff buff in buffList)
		{
			buff.OnAction();
		}
		//decide target
		switch(commandToExecute.targetType)
		{
		case TargetType.Self:
			commandToExecute.targetList.Add(this);
			break;
		case TargetType.SingleEnemy:
		case TargetType.SingleAlly:
			break;
		case TargetType.AllEnemies:
			if(this is Enemy)
				commandToExecute.targetList = new List<BattleObject>(BattleLogic.players.ToArray());
			else
				commandToExecute.targetList = new List<BattleObject>(BattleLogic.enemys.ToArray());
			break;
		case TargetType.AllAllies:
			if(this is Enemy)
				commandToExecute.targetList = new List<BattleObject>(BattleLogic.enemys.ToArray());
			else
				commandToExecute.targetList = new List<BattleObject>(BattleLogic.players.ToArray());
			break;
		}
		//decide command
		commandToExecute.source = this;
		commandToExecute.Execute();
		//post process
		timelinePosition =  -commandToExecute.postExecutionRecover;
		battleStatus = BattleStatus.Prepare;
		commandToExecute = new CommandNone();
	}

	public void RefreshAvailableCommands(BasicCommand basicCommand)
	{
		availableCommands = new List<Command> ();
		switch(basicCommand)
		{
		case BasicCommand.Attack:
			if(data.battleType != BattleType.Magical)
			{
				WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(data.weaponID);
				availableCommands.Add(new CommandUseWeaponSkill(weaponData, weaponData.skill1ID));
				availableCommands.Add(new CommandUseWeaponSkill(weaponData, weaponData.skill2ID));
				availableCommands.Add(new CommandUseWeaponSkill(weaponData, weaponData.skill3ID));
			}
			if(data.battleType != BattleType.Physical)
			{
				foreach(int magicID in data.magicIDs)
				{
					MagicData magicData = DataManager.Instance.GetItemDataSet().GetMagicData(magicID);
					availableCommands.Add(new CommandUseMagicSkill(magicData, magicData.skillID));
				}
			}
			break;
		case BasicCommand.Defence:
			availableCommands.Add(new CommandGuard());
			availableCommands.Add(new CommandEvade());
			break;
		case BasicCommand.Item:
			if(data.battleType != BattleType.Magical)
				availableCommands.Add(new CommandSwitchWeapon());
			if(GetItemCount(1) > 0)
				availableCommands.Add(new CommandUseHealing(GetItemCount(1)));
			break;
		case BasicCommand.Strategy:
			availableCommands.Add(new CommandNone());
			availableCommands.Add(new CommandEscape());
			break;
		}
		for(int i = 0; i < availableCommands.Count; i++)
		{
			availableCommands[i].commandID = i;
		}
	}

	public void AddBuff(int id)
	{
		BuffData data = DataManager.Instance.GetSkillDataSet().GetBuffData(id);
		if(Random.Range(0,101) <= data.percentage)
		{
			Buff buff = new Buff(data);
			buffList.Add(buff);
		}
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

