using UnityEngine;
using System.Collections;

public class SkillEffectSwitchAttack:SkillEffect
{
	protected override void Execute()
	{
		WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(source.GetWeapon());
		CommandUseWeaponSkill command = new CommandUseWeaponSkill(weaponData, weaponData.skill1ID);
		command.source = this.source;
		command.Execute();
	}
}
