﻿using UnityEngine;
using System.Collections;

public class CommandBlock : Command
{
	public CommandBlock()
	{
		commandType = CommandType.Defence;
		commandName = "格挡";
		commandDescription = "尝试格挡敌方接下来的攻击";
		targetType = TargetType.Self;
		preExecutionSpeed = GlobalDataStructure.BATTLE_FASTSPEED;
		postExecutionRecover = 0;
	}

	protected override void SetExecuteMessage()
	{
		executeMessage = source.GetName() + "试图格挡！";
	}

	protected override void Execute()
	{
		source.isBlocking = true;
	}
}
