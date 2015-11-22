using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BuffEffect 
{
	protected BattleObject buffSource;

	public abstract void Init(BattleObject source, string Argument);
	public abstract void Execute();
	public abstract void Revert();	
}
