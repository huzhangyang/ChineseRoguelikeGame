using UnityEngine;
using System.Collections;

public class SkillEffectFixedDamage:SkillEffect
{//固定伤害：固定伤害HP的10%
	protected override void Execute()
	{
		if(source.damage.target.IsBoss())
		{
			source.damage.minDmg = source.damage.target.maxHP * 0.05f;
			source.damage.maxDmg = source.damage.target.maxHP * 0.05f;
		}
		else
		{
			source.damage.minDmg = source.damage.target.maxHP * 0.1f;
			source.damage.maxDmg = source.damage.target.maxHP * 0.1f;
		}

	}
}
