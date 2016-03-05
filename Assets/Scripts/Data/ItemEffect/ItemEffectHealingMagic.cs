using UnityEngine;
using System.Collections;

public class ItemEffectHealingMagic:ItemEffect
{
	protected override void Execute()
	{
		if(source.GetBattleType() != BattleType.Physical)
		{
			source.maxHPMulti += 0.5f;
			BattleFormula.Heal(source, (int)(source.maxHP - source.currentHP));
			source.isHealingBottleUsed = true;
		}
	}
}
