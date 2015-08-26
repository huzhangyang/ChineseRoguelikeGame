using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum BasicCommand {Attack,Defence,Item,Strategy}
public enum BattleStatus
{
	Prepare,//等待选择行动(0~400)
	Ready,//选择行动中(400)
	Action,//即将行动(400~500)
}

public abstract class BattleObject : MonoBehaviour {
/*
 * 所有参战物体共有的数据与逻辑。
 * */	
	public BattleStatus battleStatus = BattleStatus.Prepare;
	public List<Command> availableCommands;
	public Command commandToExecute = Command.None();
	public bool isPaused = true;
	public bool isDied = false;
	private int timelinePosition;
	public int TimelinePosition//max:10000
	{
		set
		{
			if(value < 0) value = 0;
			if(value > 10000) value = 10000;
			timelinePosition = value;
			GetComponent<BattleObjectUIEvent>().SetAvatarPositionX(value / 20);//max:500
		}
		get
		{
			return timelinePosition;
		}
	}


	protected ObjectData data;

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (EventDefine.UpdateTimeline, OnUpdateTimeline);		
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (EventDefine.UpdateTimeline, OnUpdateTimeline);
	}

	protected void OnUpdateTimeline(MessageEventArgs args)
	{
		if(!this.isPaused)
			this.TimelinePosition += data.agility * 10;
		if(TimelinePosition >= 8000 && battleStatus == BattleStatus.Prepare)
		{
			SelectCommand();
		}
		if(TimelinePosition >= 10000 && battleStatus == BattleStatus.Action)
		{
			ExecuteCommand();
		}
	}

	protected virtual void SelectCommand()
	{
		TimelinePosition = 8000;
		battleStatus = BattleStatus.Ready;
	}

	protected virtual void ExecuteCommand()
	{
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", data.name);
		args.AddMessage("CommandType", ((int)commandToExecute.commandType).ToString());
		args.AddMessage("CommandName", commandToExecute.commandName);
		EventManager.Instance.PostEvent(EventDefine.ExecuteCommand, args);

		switch(commandToExecute.commandType)
		{//very temp handle!!
		case CommandType.UseSkill:
			if(commandToExecute.target != null)
				commandToExecute.target.ChangeCurrentHP(Random.Range(-100,0));
			break;
		case CommandType.Defence:
			break;
		case CommandType.UseItem:
			break;
		case CommandType.Strategy:
			break;
		case CommandType.None:
			break;
		}
		TimelinePosition = 0;
		battleStatus = BattleStatus.Prepare;
		commandToExecute = Command.None();
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
				availableCommands.Add(Command.UseSkill(weaponData.skill1ID));
				availableCommands.Add(Command.UseSkill(weaponData.skill2ID));
				availableCommands.Add(Command.UseSkill(weaponData.skill3ID));
			}
			if(data.battleType != BattleType.Physical)
			{
				foreach(int magicID in data.magicIDs)
				{
					MagicData magicData = DataManager.Instance.GetItemDataSet().GetMagicData(magicID);
					availableCommands.Add(Command.UseSkill(magicData.skillID));
				}
			}
			break;
		case BasicCommand.Defence:
			availableCommands.Add(Command.Guard());
			availableCommands.Add(Command.Evade());
			break;
		case BasicCommand.Item:
			availableCommands.Add(Command.SwitchWeapon());
			availableCommands.Add(Command.Healing());
			break;
		case BasicCommand.Strategy:
			availableCommands.Add(Command.None());
			break;
		}
		for(int i = 0; i < availableCommands.Count; i++)
		{
			availableCommands[i].commandID = i;
		}
	}

	public void ChangeCurrentHP(int hpDelta)
	{
		data.currentHP += hpDelta;
		if(data.currentHP > data.maxHP)
			data.currentHP = data.maxHP;
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", data.name);
		args.AddMessage("Damage", (-hpDelta).ToString());
		EventManager.Instance.PostEvent(EventDefine.BattleObjectHurt, args);
		if(data.currentHP <= 0)
		{
			data.currentHP = 0;
			isDied = true;
			if(this is Enemy)
				BattleLogic.enemys.Remove((Enemy)this);
			else
				BattleLogic.players.Remove((Player)this);
			MessageEventArgs args2 = new MessageEventArgs();
			args2.AddMessage("Name", data.name);
			EventManager.Instance.PostEvent(EventDefine.BattleObjectDied, args2);
			GetComponent<BattleObjectUIEvent>().DestroyAvatar();
			Destroy(this.gameObject);
		}
		GetComponent<BattleObjectUIEvent>().SetHPBar(data.currentHP, data.maxHP);
	}
}

