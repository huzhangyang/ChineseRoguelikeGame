using UnityEngine;
using System.Collections;

public class CommandUseMagicSkill : Command
{
	public CommandUseMagicSkill(MagicData magicData, int skillID)
	{
		SkillData skillData = DataManager.Instance.GetSkillDataSet ().GetSkillData (skillID);

		commandType = CommandType.UseSkill;
		commandName = skillData.name;
		commandDescription = skillData.description;
		targetType = skillData.targetType;
		preExecutionSpeed = (int)(magicData.basicSPD * skillData.preSPDMultiplier);
		postExecutionRecover = (int)(6000 / magicData.basicSPD * skillData.postSPDMultiplier);
		
		this.skillID = skillID;
	}

	public override void Execute()
	{
		SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(skillID);
		executeMessage = source.GetName() + "使用了" + skillData.name + "!";
		base.Execute();

		foreach(BattleObject target in targetList)
		{
			BattleFormula.CalculateMagicSkill(source, target, skillID);
		}
	}
}
