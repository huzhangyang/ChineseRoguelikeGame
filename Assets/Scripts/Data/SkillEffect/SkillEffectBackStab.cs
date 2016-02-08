using UnityEngine;
using System.Collections;

public class SkillEffectBackStab:SkillEffect
{//背刺：若敌人处于前摇或后摇时则强制命中，否则强制不中
	protected override void Execute()
	{
		BattleObject target = source.damage.target;
		if(target.timelinePosition == 0 ||
		   (target.timelinePosition >= GlobalDataStructure.BATTLE_TIMELINE_READY && target.timelinePosition <= GlobalDataStructure.BATTLE_TIMELINE_MAX))
		{
			source.damage.forceHit = true;
		}
		else
		{
			source.damage.forceMiss = true;
		}
	}
}
