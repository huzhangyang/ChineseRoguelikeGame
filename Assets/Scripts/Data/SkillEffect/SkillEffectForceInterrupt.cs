using UnityEngine;
using System.Collections;

public class SkillEffectForceInterrupt:SkillEffect
{
	protected override void Execute()
	{
		BattleObject target = source.damage.target;
		if(target.battleStatus == BattleStatus.Action && source.damage.interrupt > 0)
		{
			source.damage.interrupt = Mathf.Max(source.damage.interrupt, (target.timelinePosition - GlobalDataStructure.BATTLE_TIMELINE_READY ) / 100 + 1);
		}
	}
}
