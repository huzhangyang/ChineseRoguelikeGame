using UnityEngine;
using System.Collections;

public class BuffNoStrategy:Buff
{
	protected override void Init()
	{
	}
	
	protected override void Execute()
	{
		source.disableDefenceCommand = true;
		source.disableStrategyCommand = true;
	}
	
	protected override void Revert()
	{
		source.disableDefenceCommand = false;
		source.disableStrategyCommand = false;
	}
}
