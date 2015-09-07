using UnityEngine;
using System.Collections;

public class CommandNone : Command
{
	public CommandNone()
	{
		commandType = CommandType.None;
		commandName = "跳过";
		commandDescription = "什么也不做";
		targetType = TargetType.Self;
		preExecutionSpeed = GlobalDataStructure.BATTLE_MAXSPEED;
		postExecutionRecover = 0;
	}
}
