using UnityEngine;
using System.Collections;

public class SkillEffectForceHit:SkillEffect
{
	protected override void Execute()
	{
		source.damage.forceHit = true;
	}
}
