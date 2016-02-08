using UnityEngine;
using System.Collections;

public class SkillEffectSwitchAttack:SkillEffect
{//切换攻击：以切换后的武器发动轻攻击一次
	protected override void Execute()
	{
		WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(source.GetWeapon());
		CommandUseWeaponSkill command = new CommandUseWeaponSkill(weaponData, weaponData.skill1ID);
		command.source = this.source;
		command.targetList.Add(BattleManager.Instance.GetARandomEnemy(source));
		command.Execute();
	}
}
