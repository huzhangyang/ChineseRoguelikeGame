using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Command {

	public int commandID;//用于唯一标识指令（在点击基本命令后会重置）
	public CommandType commandType;//指令类型
	public string commandName;//指令名称
	public string commandDescription;//指令描述
	public TargetType targetType;//目标类型 
	public int preExecutionSpeed;//前摇
	public int postExecutionRecover;//后摇

	public int skillID;//对应的技能ID(仅在使用技能时生效)
	public int itemID;//对应的物品ID(仅在使用物品时生效)
	public BattleObject source;
	public List<BattleObject> targetList = new List<BattleObject>();
	public string executeMessage;

	public virtual void Execute()
	{
		//send message
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Message", executeMessage);
		EventManager.Instance.PostEvent(EventDefine.ExecuteCommand, args);
	}
}

public enum CommandType{None, UseSkill, Defence, UseItem, Strategy}
public enum TargetType{Self, SingleEnemy, AllEnemies, SingleAlly, AllAllies}