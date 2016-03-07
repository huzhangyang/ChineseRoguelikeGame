using UnityEngine;
using System.Collections;

public class SkillEffectInstantKill:SkillEffect
{//瞬杀:伤害强制更改为对象当前HP
	protected override void Execute()
	{
		int targetHP = source.damage.target.currentHP;
		source.damage.minDmg = targetHP;
		source.damage.maxDmg = targetHP;
		source.damage.ignoreArmor = true;
	}
}
