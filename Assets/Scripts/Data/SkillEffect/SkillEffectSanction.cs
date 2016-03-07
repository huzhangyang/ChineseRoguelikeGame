using UnityEngine;
using System.Collections;

public class SkillEffectSanction:SkillEffect
{//制裁：该回合伤害将敌方HP置为自身 + 1
	protected override void Execute()
	{
		int targetHP = source.damage.target.currentHP;
		source.damage.minDmg = targetHP - source.currentHP - 1;
		source.damage.maxDmg = targetHP - source.currentHP - 1;
		source.damage.ignoreArmor = true;
	}
}
