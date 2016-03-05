using UnityEngine;
using System;
using System.Collections;

public class CommandGuard : Command
{
	public CommandGuard()
	{
		commandType = CommandType.Defence;
		commandName = "守护";
		commandDescription = "替队友承受攻击";
		targetType = TargetType.OtherAlly;
		preExecutionSpeed = GlobalDataStructure.BATTLE_MAXSPEED;
		postExecutionRecover = 0;
	}
	
	public override void Execute()
	{
		executeMessage = String.Format("{0}正在守护{1}！",source.GetName(),targetList[0].GetName());
		SendExecuteMessage ();
		targetList[0].guardTarget = source;
		source.guardedTarget = targetList[0];
	}
}
