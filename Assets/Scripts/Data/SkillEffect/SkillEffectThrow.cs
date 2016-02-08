using UnityEngine;
using System.Collections;

public class SkillEffectThrow:SkillEffect
{//投掷：当前装备武器变为空手
	protected override void Execute()
	{
		source.SetWeapon(1000);
	}
}
