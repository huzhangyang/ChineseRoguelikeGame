using UnityEngine;
using System.Collections;

public class SkillEffectForceInterrupt:SkillEffect
{
	protected override void Execute()
	{
		BattleObject target = source.damage.target;
		if(target.timelinePosition >= GlobalDataStructure.BATTLE_TIMELINE_READY)
		{
			source.damage.interrupt = Mathf.Max(source.damage.interrupt, (target.timelinePosition - GlobalDataStructure.BATTLE_TIMELINE_READY ) / 100 + 1);
		}
	}
}
