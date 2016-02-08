using UnityEngine;
using System;
using System.Collections;

public class SkillHelper 
{
/*
 * 技能相关静态方法
 * */	
	public static void CheckSkillEffect(EffectTrigger trigger, BattleObject source)
	{
		int skillID = source.damage.skillID;
		bool isWeaponDamage = source.damage.isWeaponDamage;

		SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(skillID);
		for(int i = 0; i < skillData.effectID.Count; i++)
		{
			if(skillData.effectID[i] == 0)continue;

			int random = UnityEngine.Random.Range(0, 101);
			if(random <= skillData.effectPercent[i])
			{
				SkillEffectData effectData = DataManager.Instance.GetSkillDataSet().GetEffectData(skillData.effectID[i]);
				if(effectData.trigger == trigger)
				{
					SkillEffect.ExecuteSkillEffect(source, effectData.effectString);
				}
			}
		}

		if(isWeaponDamage)
		{
			WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(source.GetWeapon());

			for(int i = 0; i < weaponData.effectID.Count; i++)
			{
				if(weaponData.effectID[i] == 0)continue;

				int random = UnityEngine.Random.Range(0, 101);
				if(random <= weaponData.effectPercent[i])
				{
					SkillEffectData effectData = DataManager.Instance.GetSkillDataSet().GetEffectData(weaponData.effectID[i]);
					if(effectData.trigger == trigger)
					{
						SkillEffect.ExecuteSkillEffect(source, effectData.effectString);
					}
				}
			}
		}
	}

	public static void CheckBuff(BuffTrigger trigger, BattleObject source)
	{
		for(int i = 0; i < source.buffList.Count; i++)
		{
			source.buffList[i].Check(trigger);
		}
	}
	
}
