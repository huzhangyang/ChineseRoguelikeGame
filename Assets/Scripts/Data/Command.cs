using UnityEngine;
using System.Collections;

public class Command {

	public int commandID;//用于唯一标识指令（在点击基本命令后会重置）
	public CommandType commandType;//指令类型
	public string commandName;//指令名称
	public string commandDescription;//指令描述
	public TargetType targetType;//目标类型 
	public int preExecutionSpeed;//前摇
	public int postExecutionRecover;//后摇

	public int skillOrItemID;//对应的物品或技能ID(仅在使用技能或物品时生效)
	public BattleObject target;//目标（仅在单个目标时生效）

	public static Command None()
	{
		Command command = new Command ();
		command.commandType = CommandType.None;
		command.commandName = "跳过";
		command.commandDescription = "什么也不做";
		command.targetType = TargetType.Self;
		command.preExecutionSpeed = GlobalDataStructure.BATTLE_MAXSPEED;
		command.postExecutionRecover = 0;
		return command;
	}

	public static Command Guard()
	{
		Command command = new Command ();
		command.commandType = CommandType.Defence;
		command.commandName = "格挡";
		command.commandDescription = "尝试格挡敌方接下来的攻击";
		command.targetType = TargetType.Self;
		command.preExecutionSpeed = GlobalDataStructure.BATTLE_STANDARDSPEED;
		command.postExecutionRecover = 0;
		return command;
	}

	public static Command Evade()
	{
		Command command = new Command ();
		command.commandType = CommandType.Defence;
		command.targetType = TargetType.Self;
		command.commandName = "闪避";
		command.commandDescription = "尝试躲避敌方接下来的攻击";
		command.preExecutionSpeed = GlobalDataStructure.BATTLE_STANDARDSPEED;
		command.postExecutionRecover = 0;
		return command;
	}

	public static Command SwitchWeapon()
	{
		Command command = new Command ();
		command.commandType = CommandType.UseItem;
		command.commandName = "切换武器";
		command.commandDescription = "切换当前使用的武器";
		command.targetType = TargetType.Self;
		command.preExecutionSpeed = GlobalDataStructure.BATTLE_STANDARDSPEED;
		command.postExecutionRecover = 0;

		command.skillOrItemID = 1000 + Random.Range(1,6) * 100;
		return command;
	}

	public static Command Healing(int itemCount)
	{
		Command command = new Command ();
		command.commandType = CommandType.UseItem;
		command.commandName = "原力之瓶(" + itemCount + ")";
		command.commandDescription = "使用原力之瓶来回复生命值";
		command.targetType = TargetType.SingleAlly;
		command.preExecutionSpeed = GlobalDataStructure.BATTLE_MINSPEED;
		command.postExecutionRecover = 0;

		command.skillOrItemID = 1;
		return command;
	}

	public static Command Escape()
	{
		Command command = new Command ();
		command.commandType = CommandType.Strategy;
		command.commandName = "逃跑";
		command.commandDescription = "逃命！";
		command.targetType = TargetType.Self;
		command.preExecutionSpeed = GlobalDataStructure.BATTLE_MINSPEED;
		command.postExecutionRecover = 0;

		return command;
	}

	public static Command UseWeaponSkill(WeaponData weaponData, int skillID)
	{
		SkillData skillData = DataManager.Instance.GetSkillDataSet ().GetSkillData (skillID);

		Command command = new Command ();
		command.commandType = CommandType.UseSkill;
		command.commandName = skillData.name;
		command.commandDescription = skillData.description;
		command.targetType = skillData.targetType;
		command.preExecutionSpeed = (int)(weaponData.basicSPD * skillData.preSPDMultiplier);
		command.postExecutionRecover = (int)(weaponData.basicSPD * skillData.postSPDMultiplier);

		command.skillOrItemID = skillID;
		return command;
	}

	public static Command UseMagicSkill(MagicData magicData, int skillID)
	{
		SkillData skillData = DataManager.Instance.GetSkillDataSet ().GetSkillData (skillID);
		
		Command command = new Command ();
		command.commandType = CommandType.UseSkill;
		command.commandName = skillData.name;
		command.commandDescription = skillData.description;
		command.targetType = skillData.targetType;
		command.preExecutionSpeed = (int)(magicData.basicSPD * skillData.preSPDMultiplier);
		command.postExecutionRecover = (int)(magicData.basicSPD * skillData.postSPDMultiplier);

		command.skillOrItemID = skillID;
		return command;
	}
}

public enum CommandType{None, UseSkill, Defence, UseItem, Strategy}
public enum TargetType{Self, SingleEnemy, AllEnemies, SingleAlly, AllAllies}