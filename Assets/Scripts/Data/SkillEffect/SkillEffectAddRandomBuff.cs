using UnityEngine;
using System.Collections;

public class SkillEffectAddRandomBuff:SkillEffect
{//随机为目标附加BUFF
	protected override void Execute()
	{
		int randomBuffID = Random.Range(2, 15);
		int randomTurns = Random.Range(1, 5);
		source.damage.target.AddBuff(randomBuffID, randomTurns);
	}
}
