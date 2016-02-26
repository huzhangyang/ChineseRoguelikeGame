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

	public override void Execute()
	{
		executeMessage = source.GetName() + "拿出了 " + DataManager.Instance.GetItemDataSet().GetWeaponData(itemID).name + "！";
		SendExecuteMessage ();
		source.SetWeapon(itemID);

		SkillHelper.CheckWeaponEffect(EffectTrigger.SwitchWeapon, source);//检查切换武器时生效的特效
	}
}
