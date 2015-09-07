using UnityEngine;
using System.Collections;

public class CommandUseHealing : Command
{
	public CommandUseHealing(int itemCount)
	{
		commandType = CommandType.UseItem;
		commandName = "原力之瓶(" + itemCount + ")";
		commandDescription = "使用原力之瓶来回复生命值";
		targetType = TargetType.SingleAlly;
		preExecutionSpeed = GlobalDataStructure.BATTLE_MINSPEED;
		postExecutionRecover = 0;
		
		itemID = 1;
	}

	public override void Execute()
	{
		base.Execute();
		foreach(BattleObject target in targetList)
		{
			target.Heal((int)(target.GetData().maxHP * 0.8f - target.GetData().currentHP));
			source.GetData().ConsumeItem(itemID);
		}
	}
}
