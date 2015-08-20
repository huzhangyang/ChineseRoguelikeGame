using UnityEngine;
using System.Collections;

public class Command {

	public string commandType;
	public string commandName;
	public string commandDescription;

	public static Command None()
	{
		Command command = new Command ();
		command.commandType = "None";
		command.commandName = "None";
		command.commandDescription = "None";
		return command;
	}

	public static Command Build(string type, string name, string desc)
	{
		Command command = new Command ();
		command.commandType = type;
		command.commandName = name;
		command.commandDescription = desc;
		return command;
	}

	public static Command BuildWithSkillID(int skillID)
	{
		SkillData skillData = DataManager.Instance.GetSkillDataSet ().GetSkillData (skillID);

		Command command = new Command ();
		command.commandType = "Skill";
		command.commandName = skillData.name;
		command.commandDescription = skillData.name + "技能描述";
		return command;
	}
}
