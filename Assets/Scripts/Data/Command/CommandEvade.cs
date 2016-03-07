using UnityEngine;
using System.Collections;

public class CommandEvade : Command
{
	public CommandEvade()
	{
		commandType = CommandType.Defence;
		commandName = "闪避";
		commandDescription = "尝试躲避敌方接下来的攻击";
		targetType = TargetType.Self;
		preExecutionSpeed = GlobalDataStructure.BATTLE_FASTSPEED;
		postExecutionRecover = 0;
	}

	protected override void SetExecuteMessage()
	{
		executeMessage = source.GetName() + "试图躲避！";
	}

	protected override void Execute()
	{
		source.isEvading = true;
	}
}
