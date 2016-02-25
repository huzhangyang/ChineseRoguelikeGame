using UnityEngine;
using System.Collections;

public class SkillEffectCombo10:SkillEffect
{
	protected override void Execute()
	{
		source.damage.combo = 10;
		source.damage.stopComboOnMiss = true;
	}
}
