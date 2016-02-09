using UnityEngine;
using System.Collections;

public class SkillEffectCombo2:SkillEffect
{
	protected override void Execute()
	{
		BattleFormula.OnDamage(source.damage.target, source.damage.dmg, source.damage.interrupt);
	}
}
