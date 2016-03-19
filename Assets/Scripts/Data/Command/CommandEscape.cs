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

	protected override void SetExecuteMessage()
	{
		executeMessage = source.GetName() + "逃跑了！";
	}

	protected override void Execute()
	{
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Object", source);
		EventManager.Instance.PostEvent(BattleEvent.BattleObjectEscape, args);
	}
}
