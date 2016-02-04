using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public abstract class SkillEffect 
{
	protected BattleObject source;	
	protected abstract void Execute();

	public static void ExecuteSkillEffect(BattleObject bo,string effectName)
	{
		SkillEffect effect = (SkillEffect)Assembly.GetExecutingAssembly().CreateInstance("SkillEffect" + effectName, true); 
		effect.source = bo;
		effect.Execute();
	}
}
