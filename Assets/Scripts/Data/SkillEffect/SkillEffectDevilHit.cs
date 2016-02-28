using UnityEngine;
using System.Collections;

public class SkillEffectDevilHit:SkillEffect
{//厄运一击:若对方HP低于自己则秒杀敌人，否则伤害自己。
	protected override void Execute()
	{
		int targetHP = source.damage.target.currentHP;
		if(targetHP < source.currentHP)
		{
			source.damage.minDmg = targetHP;
			source.damage.maxDmg = targetHP;
		}
		else
		{
			source.damage.target = source;
		}
	}
}
