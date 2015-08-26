using UnityEngine;
using System.Collections;

public class Command {

	public int commandID;//用于唯一标识指令（在点击基本命令后会重置）
	public CommandType commandType;//指令类型
	public TargetType targetType;//目标类型 
	public string commandName;//指令名称
	public string commandDescription;
	public int skillOrItemID;
	public BattleObject target;//目标（仅在单个目标时生效）

	public static Command None()
	{
		Command command = new Command ();
		command.commandType = CommandType.None;
		command.targetType = TargetType.Self;
		command.commandName = "跳过";
		command.commandDescription = "什么也不做";
		return command;
	}

	public static Command Guard()
	{
		Command command = new Command ();
		command.commandType = CommandType.Defence;
		command.targetType = TargetType.Self;
		command.commandName = "格挡";
		command.commandDescription = "尝试格挡敌方接下来的攻击";
		return command;
	}

	public static Command Evade()
	{
		Command command = new Command ();
		command.commandType = CommandType.Defence;
		command.targetType = TargetType.Self;
		command.commandName = "闪避";
		command.commandDescription = "尝试躲避敌方接下来的攻击";
		return command;
	}

	public static Command SwitchWeapon()
	{
		Command command = new Command ();
		command.commandType = CommandType.UseItem;
		command.targetType = TargetType.Self;
		command.commandName = "切换武器";
		command.commandDescription = "切换当前使用的武器";
		command.skillOrItemID = 1000 + Random.Range(1,6) * 100;
		return command;
	}

	public static Command Healing(int itemCount)
	{
		Command command = new Command ();
		command.commandType = CommandType.UseItem;
		command.targetType = TargetType.SingleAlly;
		command.commandName = "原力之瓶(" + itemCount + ")";
		command.commandDescription = "使用原力之瓶来回复生命值";
		command.skillOrItemID = 1;
		return command;
	}

	public static Command Escape()
	{
		Command command = new Command ();
		command.commandType = CommandType.Strategy;
		command.targetType = TargetType.Self;
		command.commandName = "逃跑";
		command.commandDescription = "逃命！";
		return command;
	}

	public static Command UseSkill(int skillID)
	{
		SkillData skillData = DataManager.Instance.GetSkillDataSet ().GetSkillData (skillID);

		Command command = new Command ();
		command.commandType = CommandType.UseSkill;
		command.targetType = skillData.targetType;
		command.commandName = skillData.name;
		command.commandDescription = skillData.description;
		command.skillOrItemID = skillID;
		return command;
	}
}

public enum CommandType{None, UseSkill, Defence, UseItem, Strategy}
public enum TargetType{Self, SingleEnemy, AllEnemies, SingleAlly, AllAllies}