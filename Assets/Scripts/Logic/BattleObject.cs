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

	public int timelinePosition
	{
		set
		{
			timelineAvatar.rectTransform.anchoredPosition = new Vector2(value, 0);
		}
		get
		{
			return (int)timelineAvatar.rectTransform.anchoredPosition.x;
		}
	}//max:500

	protected ObjectData data;
	protected Image timelineAvatar;
	protected Slider HPBar;

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
			this.timelinePosition += data.agility;
		if(timelinePosition >= 400 && battleStatus == BattleStatus.Prepare)
		{
			SelectCommand();
		}
		if(timelinePosition >= 500 && battleStatus == BattleStatus.Action)
		{
			ExecuteCommand();
		}
	}

	protected virtual void SelectCommand()
	{
		timelinePosition = 400;
		battleStatus = BattleStatus.Ready;
	}

	protected virtual void ExecuteCommand()
	{
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", data.name);
		args.AddMessage("CommandType", commandToExecute.commandType.ToString());
		args.AddMessage("CommandName", commandToExecute.commandName);
		EventManager.Instance.PostEvent(EventDefine.ExecuteCommand, args);

		timelinePosition = 0;
		battleStatus = BattleStatus.Prepare;
		commandToExecute = Command.None();
	}

	protected void SetHPBar()
	{
		HPBar = this.gameObject.GetComponentInChildren<Slider> ();
		HPBar.maxValue = data.maxHP;
		HPBar.value = data.currentHP;
	}

	public void SetAvatar(GameObject avatar)
	{
		timelineAvatar = avatar.GetComponent<Image>();
		timelineAvatar.sprite = this.GetComponent<Image> ().sprite;
		timelineAvatar.rectTransform.anchoredPosition = Vector2.zero;
	}

	public void RefreshAvailableCommands(BasicCommand basicCommand)
	{
		availableCommands = new List<Command> ();
		switch(basicCommand)
		{
		case BasicCommand.Attack:
			WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(data.weaponID);
			availableCommands.Add(Command.BuildWithSkillID(weaponData.skill1ID));
			availableCommands.Add(Command.BuildWithSkillID(weaponData.skill2ID));
			availableCommands.Add(Command.BuildWithSkillID(weaponData.skill3ID));
			break;
		case BasicCommand.Defence:
			break;
		case BasicCommand.Item:
			break;
		case BasicCommand.Strategy:
			break;
		}
		for(int i = 0; i < availableCommands.Count; i++)
		{
			availableCommands[i].commandID = i;
		}
	}

}

