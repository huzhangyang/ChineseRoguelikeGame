using UnityEngine;
using System.Collections;

public class SkillEffectIgnoreDefence:SkillEffect
{
	protected override void Execute()
	{
		source.damage.ignoreDefence = true;
	}
}
