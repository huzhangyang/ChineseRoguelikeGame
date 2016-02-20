using UnityEngine;
using System.Collections;

public class CommandSwitchWeapon : Command
{
	public CommandSwitchWeapon()
	{
		commandType = CommandType.Item;
		commandName = "切换武器";
		commandDescription = "切换当前使用的武器";
		targetType = TargetType.Self;
		preExecutionSpeed = GlobalDataStructure.BATTLE_STANDARDSPEED;
		postExecutionRecover = 0;
		
		itemID = 1100;
	}

	public override void Execute()
	{
		executeMessage = source.GetName() + "拿出了 " + DataManager.Instance.GetItemDataSet().GetWeaponData(itemID).name + "！";
		SendExecuteMessage ();
		source.SetWeapon(itemID);

		SkillHelper.CheckWeaponEffect(EffectTrigger.SwitchWeapon, source);//检查切换武器时生效的特效
	}
}
