using UnityEngine;
using System.Collections;

public class ItemEffectHealingPhysic:ItemEffect
{
	protected override void Execute()
	{
		BattleFormula.Heal(source, (int)(source.maxHP * GlobalDataStructure.HP_RECOVER_THRESHOLD - source.currentHP));
	}
}
