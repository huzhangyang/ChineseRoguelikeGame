using UnityEngine;
using System.Collections;

public class SkillEffectCombo2:SkillEffect
{
	protected override void Execute()
	{
		source.damage.combo = 2;
	}
}
