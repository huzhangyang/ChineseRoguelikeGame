using UnityEngine;
using System.Collections;

public class SkillEffectIgnoreArmor:SkillEffect
{
	protected override void Execute()
	{
		source.damage.ignoreArmor = true;
	}
}

