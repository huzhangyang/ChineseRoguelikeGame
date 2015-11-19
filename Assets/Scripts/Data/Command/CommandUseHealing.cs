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
		if(targetList.Equals(source))
			executeMessage = source.GetName() + "喝下了原力之瓶！";
		else
			executeMessage = source.GetName() + "给" + targetList[0].GetName() +"喝下了原力之瓶！";
		base.Execute();
		foreach(BattleObject target in targetList)
		{
			BattleFormula.Heal(target, (int)(target.maxHP * GlobalDataStructure.HP_RECOVER_THRESHOLD - target.currentHP));
			source.ConsumeItem(itemID);
		}
	}
}
