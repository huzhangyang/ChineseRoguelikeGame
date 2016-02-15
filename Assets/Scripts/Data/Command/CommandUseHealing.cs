using UnityEngine;
using System.Collections;

public class CommandUseHealing : Command
{
	public CommandUseHealing(int itemCount)
	{
		commandType = CommandType.Item;
		commandName = "灵气之壶(" + itemCount + ")";
		commandDescription = "服下灵气结晶，提升生命力";
		targetType = TargetType.Self;
		preExecutionSpeed = GlobalDataStructure.BATTLE_MINSPEED;
		postExecutionRecover = 0;
		
		itemID = 1;
	}

	public override void Execute()
	{
		executeMessage = source.GetName() + "服下了灵气结晶！";
		SendExecuteMessage ();
		foreach(BattleObject target in targetList)
		{
			if(target.GetBattleType() == BattleType.Magical)
			{
				target.maxHPMulti += 0.5f;
			}
			else
			{
				BattleFormula.Heal(target, (int)(target.maxHP * GlobalDataStructure.HP_RECOVER_THRESHOLD - target.currentHP));
			}
			source.ConsumeItem(itemID);
		}
	}
}
