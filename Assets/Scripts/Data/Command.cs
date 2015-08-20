using UnityEngine;
using System.Collections;

public class Command {

	public string commandType;
	public string commandName;
	public string commandDescription;

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
