using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum BasicCommand {Attack,Defence,Item,Strategy}
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
	public int maxHP{get{return data.maxHP * maxHPMulti + maxHPInc;}}
	public int maxHPInc = 0;
	public int maxHPMulti = 1;

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

	public int power{get{return data.power * powerMulti + powerInc;}}
	public int powerInc = 0;
	public int powerMulti = 1;
	
	public int agility{get{return data.agility * agilityMulti + agilityInc;}}
	public int agilityInc = 0;
	public int agilityMulti = 1;
	
	public int toughness{get{return data.toughness * toughnessInc + toughnessMulti;}}
	public int toughnessInc = 0;
	public int toughnessMulti = 1;
	
	public int insight{get{return data.insight * insightInc + insightMulti;}}
	public int insightInc = 0;
	public int insightMulti = 1;
	
	public int skill{get{return data.skill * skillInc + skillMulti;}}
	public int skillInc = 0;
	public int skillMulti = 1;
	
	public int luck{get{return data.luck * luckInc + luckMulti;}}
	public int luckInc = 0;
	public int luckMulti = 1;
	
	public int eloquence{get{return data.eloquence * eloquenceInc + eloquenceMulti;}}
	public int eloquenceInc = 0;
	public int eloquenceMulti = 1;	

	public BattleStatus battleStatus = BattleStatus.Prepare;
	public List<Command> availableCommands;
	public List<Buff> buffList;
	public Command commandToExecute = new CommandNone();
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
		Buff buff = new Buff(data);
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

