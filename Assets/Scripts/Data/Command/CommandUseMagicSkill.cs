using UnityEngine;
using System.Collections;

public class CommandUseMagicSkill : Command
{
	public CommandUseMagicSkill(MagicData magicData, int skillID)
	{
		SkillData skillData = DataManager.Instance.GetSkillDataSet ().GetSkillData (skillID);

		commandType = CommandType.Attack;
		commandName = skillData.name;
		commandDescription = skillData.description;
		targetType = skillData.targetType;
		preExecutionSpeed = (int)(magicData.basicSPD * skillData.preSPDMultiplier / 3);
		postExecutionRecover = 0;
		
		this.skillID = skillID;
	}

	public override void Execute()
	{
		MagicData magicData = DataManager.Instance.GetItemDataSet().GetMagicDataBySkillID(skillID);
		SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(skillID);
		float cost = magicData.cost >= 1 ? magicData.cost :source.maxHP * magicData.cost;
		source.currentHP -= Mathf.RoundToInt(cost);

		executeMessage = source.GetName() + "使用了" + skillData.name + "!\n" + "消耗了" + cost + "点灵力!" ;
		SendExecuteMessage ();

		foreach(BattleObject target in targetList)
		{
			BattleFormula.CalculateSkill(source, target, skillID, false);
		}
	}
}
