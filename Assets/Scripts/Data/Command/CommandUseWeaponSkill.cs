using UnityEngine;
using System;
using System.Collections;

public class CommandUseWeaponSkill : Command
{
	public CommandUseWeaponSkill(WeaponData weaponData, int skillID)
	{
		SkillData skillData = DataManager.Instance.GetSkillDataSet ().GetSkillData (skillID);

		commandType = CommandType.Attack;
		commandName = skillData.name;
		commandDescription = String.Format(skillData.description, "<color=yellow>" + skillData.ATKMultiplier * 100 + "%</color>", "<color=yellow>" + weaponData.interrupt * skillData.interruptMultiplier + "%</color>");
		targetType = skillData.targetType;
		preExecutionSpeed = Mathf.RoundToInt(weaponData.basicSPD * skillData.preSPDMultiplier);
		postExecutionRecover = skillData.postSPDMultiplier == 0 ? 0 : Mathf.RoundToInt(6000f / weaponData.basicSPD / skillData.postSPDMultiplier);
		
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
