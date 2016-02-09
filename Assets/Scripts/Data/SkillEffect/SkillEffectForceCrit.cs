using UnityEngine;
using System.Collections;

public class SkillEffectForceCrit:SkillEffect
{
	protected override void Execute()
	{
		source.damage.forceCrit = true;
	}
}
