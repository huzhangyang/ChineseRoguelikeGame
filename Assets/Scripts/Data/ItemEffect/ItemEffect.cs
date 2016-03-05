using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public abstract class ItemEffect 
{
	protected BattleObject source;	
	protected abstract void Execute();
	
	public static void ExecuteItemEffect(BattleObject bo,string effectName)
	{
		ItemEffect effect = (ItemEffect)Assembly.GetExecutingAssembly().CreateInstance("ItemEffect" + effectName, true); 
		effect.source = bo;
		effect.Execute();
	}
}
