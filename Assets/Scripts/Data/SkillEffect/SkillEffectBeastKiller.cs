using UnityEngine;
using System.Collections;

public class SkillEffectBeastKiller:SkillEffect
{//若对象带有“兽”Tag，则伤害 + 25%。若对象为灵气型，则伤害为0
	protected override void Execute()
	{
		if(source.damage.target.GetBattleType() == BattleType.Magical)
		{
			source.damage.dmg = 0;
		}
	}
}
