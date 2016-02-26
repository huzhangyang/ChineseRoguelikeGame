using UnityEngine;
using System.Collections;

public class SkillEffectMoneyDamage:SkillEffect
{//金钱伤害：根据当前我方金钱决定伤害值
	protected override void Execute()
	{//TODO
		source.damage.dmg = 648;		
	}
}
