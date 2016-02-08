using UnityEngine;
using System.Collections;

public class SkillEffectAbsorbDamage:SkillEffect
{//吸血：吸取伤害的10%回复到自己生命
	protected override void Execute()
	{
		int absorbDamage = Mathf.RoundToInt(source.damage.dmg * 0.1f);
		BattleFormula.Heal (source, absorbDamage);
	}
}
