using UnityEngine;
using System.Collections;

public class SkillEffectDevilHit:SkillEffect
{//厄运一击:若对方HP低于自己则有50%几率秒杀敌人，否则有50%几率伤害自己。
	protected override void Execute()
	{
		int number = Random.Range(0, 101);
		if(number <= 50)
		{
			int targetHP = source.damage.target.currentHP;
			if(targetHP < source.currentHP)
			{
				source.damage.dmg = targetHP;
			}
			else
			{
				source.damage.isCountered = true;
			}
		}
	}
}
