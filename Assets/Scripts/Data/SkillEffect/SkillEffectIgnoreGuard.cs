using UnityEngine;
using System.Collections;

public class SkillEffectIgnoreGuard:SkillEffect
{
	protected override void Execute()
	{
		source.damage.ignoreGuard = true;
	}
}
