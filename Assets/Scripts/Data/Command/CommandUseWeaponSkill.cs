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
		ObjectData data = source.GetData();
		SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(skillID);
		WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(data.weaponID);
		executeMessage = data.name + "使用了" + skillData.name + "!";
		base.Execute();

		foreach(BattleObject target in targetList)
		{
			float hit = data.skill + data.luck / 10.0f + weaponData.basicACC * skillData.ACCMultiplier;//命中率
			float crit = data.skill / 10.0f + data.luck / 10.0f + weaponData.basicCRT * skillData.CRTMultiplier / 100.0f;//暴击率
			float damage = (data.power + weaponData.basicATK) * skillData.ATKMultiplier;//伤害值
			target.OnBeHit(source, skillData.skillType, hit, crit, damage);
		}
	}
}
