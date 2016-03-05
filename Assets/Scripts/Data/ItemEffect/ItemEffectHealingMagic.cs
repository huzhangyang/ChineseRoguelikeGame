using UnityEngine;
using System.Collections;

public class ItemEffectHealingMagic:ItemEffect
{
	protected override void Execute()
	{
		if(source.GetBattleType() != BattleType.Physical)
		{
			source.maxHPMulti += 0.5f;
			source.currentHP = source.maxHP;
			source.isHealingBottleUsed = true;
		}
	}
}
