using UnityEngine;
using System.Collections;

public class BuffEndure:Buff
{	
	protected override void Init()
	{
	}
	
	protected override void Execute()
	{
		source.damageTaken.interrupt = 0;
	}
	
	protected override void Revert()
	{
	}
}
