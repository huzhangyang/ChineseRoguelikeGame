using UnityEngine;
using System.Collections;

public class CommandEscape : Command
{
	public CommandEscape()
	{
		commandType = CommandType.Strategy;
		commandName = "逃跑";
		commandDescription = "逃命！";
		targetType = TargetType.Self;
		preExecutionSpeed = GlobalDataStructure.BATTLE_MINSPEED;
		postExecutionRecover = 0;
	}

	public override void Execute()
	{
		base.Execute();
		EventManager.Instance.PostEvent(EventDefine.BattleObjectEscape);
	}
}
