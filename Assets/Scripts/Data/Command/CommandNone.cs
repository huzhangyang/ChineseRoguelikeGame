using UnityEngine;
using System.Collections;

public class CommandNone : Command
{
	public CommandNone()
	{
		commandType = CommandType.Strategy;
		commandName = "跳过";
		commandDescription = "什么也不做";
		targetType = TargetType.Self;
		preExecutionSpeed = GlobalDataStructure.BATTLE_MAXSPEED;
		postExecutionRecover = 0;
	}

	protected override void SetExecuteMessage()
	{
		executeMessage = source.GetName() + "按兵不动！";
	}

	protected override void Execute()
	{
	}
}
