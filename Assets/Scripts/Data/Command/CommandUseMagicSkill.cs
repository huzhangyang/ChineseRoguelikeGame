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
		preExecutionSpeed = Mathf.RoundToInt(magicData.basicSPD * skillData.preSPDMultiplier);
		postExecutionRecover = skillData.postSPDMultiplier == 0 ? 0 : Mathf.RoundToInt(6000f / magicData.basicSPD / skillData.postSPDMultiplier);
		
		this.skillID = skillID;
	}

	protected override void SetExecuteMessage()
	{
		SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(skillID);
		
		if(source.GetBattleType() == BattleType.Magical)
		{
			MagicData magicData = DataManager.Instance.GetItemDataSet().GetMagicDataBySkillID(skillID);
			float cost = magicData.cost >= 1 ? magicData.cost :source.maxHP * magicData.cost;
			source.currentHP -= Mathf.RoundToInt(cost);
			executeMessage = source.GetName() + "使用了" + skillData.name + "!\n" + "消耗了" + cost + "点灵力!" ;
		}
		else
		{
			executeMessage = source.GetName() + "使用了" + skillData.name + "!" ;
		}
	}

	protected override void Execute()
	{
		foreach(BattleObject target in targetList)
		{
			BattleFormula.CalculateSkill(source, target, skillID, false);
		}
	}
}
