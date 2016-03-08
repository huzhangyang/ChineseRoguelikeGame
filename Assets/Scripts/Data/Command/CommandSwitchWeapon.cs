using UnityEngine;
using System;
using System.Collections;

public class CommandSwitchWeapon : Command
{
	public CommandSwitchWeapon(int weaponID)
	{
		WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(weaponID);
		commandType = CommandType.Item;
		commandName = String.Format("切换武器({0})", weaponData.name);
		commandDescription = "切换当前使用的武器为 " + weaponData.name;

		targetType = TargetType.Self;
		preExecutionSpeed = GlobalDataStructure.BATTLE_STANDARDSPEED;
		postExecutionRecover = 0;
		
		itemID = weaponID;
	}

	protected override void SetExecuteMessage()
	{
		WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(itemID);
		executeMessage = String.Format("{0}拿出了{1}!", source.GetName(), weaponData.name);
	}

	protected override void Execute()
	{
		int offID = source.GetWeapon ();
		source.SetWeapon(itemID);
		SkillHelper.CheckWeaponBuff (source, offID);
		SkillHelper.CheckWeaponEffect(EffectTrigger.SwitchWeapon, source);//检查切换武器时生效的特效
	}
}
