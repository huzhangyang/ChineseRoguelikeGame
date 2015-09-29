using UnityEngine;
using System.Collections;

public class CommandUseWeaponSkill : Command
{
	public CommandUseWeaponSkill(WeaponData weaponData, int skillID)
	{
		SkillData skillData = DataManager.Instance.GetSkillDataSet ().GetSkillData (skillID);

		commandType = CommandType.UseSkill;
		commandName = skillData.name;
		commandDescription = skillData.description;
		targetType = skillData.targetType;
		preExecutionSpeed = (int)(weaponData.basicSPD * skillData.preSPDMultiplier);
		postExecutionRecover = (int)(6000 / weaponData.basicSPD * skillData.postSPDMultiplier);
		
		this.skillID = skillID;
	}

	public override void Execute()
	{
		SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(skillID);
		executeMessage = source.GetName() + "使用了" + skillData.name + "!";
		base.Execute();

		foreach(BattleObject target in targetList)
		{
			BattleFormula.CalculateWeaponSkill(source, target, skillID);
		}
	}
}
