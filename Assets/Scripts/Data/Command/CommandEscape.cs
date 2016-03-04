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
		preExecutionSpeed = GlobalDataStructure.BATTLE_SLOWSPEED;
		postExecutionRecover = 0;
	}

	public override void Execute()
	{
		executeMessage = source.GetName() + "逃跑了！";
		SendExecuteMessage ();
		EventManager.Instance.PostEvent(BattleEvent.BattleObjectEscape);
	}
}
