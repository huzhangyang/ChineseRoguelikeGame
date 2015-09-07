using UnityEngine;
using System.Collections;

public class CommandEvade : Command
{
	public CommandEvade()
	{
		commandType = CommandType.Defence;
		targetType = TargetType.Self;
		commandName = "闪避";
		commandDescription = "尝试躲避敌方接下来的攻击";
		preExecutionSpeed = GlobalDataStructure.BATTLE_STANDARDSPEED;
		postExecutionRecover = 0;
	}

	public override void Execute()
	{
		base.Execute();
		source.isEvading = true;
	}
}
