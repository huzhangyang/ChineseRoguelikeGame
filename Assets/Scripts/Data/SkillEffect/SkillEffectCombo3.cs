using UnityEngine;
using System.Collections;

public class SkillEffectCombo3:SkillEffect
{
	protected override void Execute()
	{
		source.damage.combo = 3;
	}
}
