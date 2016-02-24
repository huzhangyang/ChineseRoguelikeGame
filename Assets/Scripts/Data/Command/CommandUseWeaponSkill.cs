using UnityEngine;
using System.Collections;

public class CommandUseWeaponSkill : Command
{
	public CommandUseWeaponSkill(WeaponData weaponData, int skillID)
	{
		SkillData skillData = DataManager.Instance.GetSkillDataSet ().GetSkillData (skillID);

		commandType = CommandType.Attack;
		commandName = skillData.name;
		commandDescription = skillData.description;
		targetType = skillData.targetType;
		preExecutionSpeed = Mathf.RoundToInt(weaponData.basicSPD * skillData.preSPDMultiplier);
		postExecutionRecover = Mathf.RoundToInt(weaponData.basicSPD / skillData.postSPDMultiplier * 0.6f);
		
		this.skillID = skillID;
	}

	public override void Execute()
	{
		SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(skillID);
		executeMessage = source.GetName() + "使用了" + skillData.name + "!";
		SendExecuteMessage ();

		foreach(BattleObject target in targetList)
		{
			BattleFormula.CalculateSkill(source, target, skillID, true);
		}
	}
}
