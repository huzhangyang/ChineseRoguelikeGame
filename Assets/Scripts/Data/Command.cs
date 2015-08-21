﻿using UnityEngine;
using System.Collections;

public class Command {

	public int commandID;//用于唯一标识指令（在点击基本命令后会重置）
	public CommandType commandType;//指令类型
	public string commandName;//指令名称
	public string commandDescription;
	public int skillID;
	public BattleObject target;

	public static Command None()
	{
		Command command = new Command ();
		command.commandType = CommandType.None;
		command.commandName = "None";
		command.commandDescription = "None";
		return command;
	}

	public static Command BuildWithSkillID(int skillID)
	{
		SkillData skillData = DataManager.Instance.GetSkillDataSet ().GetSkillData (skillID);

		Command command = new Command ();
		command.commandType = CommandType.UseSkill;
		command.commandName = skillData.name;
		command.commandDescription = skillData.name + "技能描述";
		command.skillID = skillID;
		return command;
	}
}

public enum CommandType{None, UseSkill}