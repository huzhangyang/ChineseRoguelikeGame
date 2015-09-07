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
		executeMessage = source.GetData().name + "逃跑了！";
		base.Execute();
		EventManager.Instance.PostEvent(EventDefine.BattleObjectEscape);
	}
}
