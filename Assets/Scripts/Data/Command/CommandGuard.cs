using UnityEngine;
using System.Collections;

public class CommandGuard : Command
{
	public CommandGuard()
	{
		commandType = CommandType.Defence;
		commandName = "格挡";
		commandDescription = "尝试格挡敌方接下来的攻击";
		targetType = TargetType.Self;
		preExecutionSpeed = GlobalDataStructure.BATTLE_STANDARDSPEED;
		postExecutionRecover = 0;
	}

	public override void Execute()
	{
		executeMessage = source.GetName() + "试图格挡！";
		base.Execute();
		source.isGuarding = true;
	}
}
