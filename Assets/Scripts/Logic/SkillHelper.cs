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
			CheckWeaponEffect(trigger, source);
		}
	}

	public static void CheckWeaponEffect(EffectTrigger trigger, BattleObject source)
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

	public static void CheckBuff(BuffTrigger trigger, BattleObject source)
	{
		for(int i = 0; i < source.buffList.Count; i++)
		{
			source.buffList[i].Check(trigger);
		}
	}

	public static void CheckBuffAdd(BattleObject source)
	{
		int skillID = source.damage.skillID;
		bool isWeaponDamage = source.damage.isWeaponDamage;

		SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(skillID);
		for(int i = 0; i < skillData.buffID.Count; i++)
		{
			if(skillData.buffID[i] == 0)continue;
			
			int random = UnityEngine.Random.Range(0, 101);
			if(random <= skillData.buffPercent[i])
			{
				source.damage.target.AddBuff(skillData.buffID[i], skillData.buffTurns[i]);
			}
		}
		
		if(isWeaponDamage)
		{
			WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(source.GetWeapon());
			
			for(int i = 0; i < weaponData.buffID.Count; i++)
			{
				if(weaponData.buffID[i] == 0)continue;
				
				int random = UnityEngine.Random.Range(0, 101);
				if(random <= weaponData.buffPercent[i])
				{
					source.damage.target.AddBuff(weaponData.buffID[i], weaponData.buffTurns[i]);
				}
			}
		}
	}

	public static bool FillCommandTarget(BattleObject source)
	{
		switch(source.commandToExecute.targetType)
		{
		case TargetType.SingleEnemy:
		case TargetType.SingleAlly:
			return false;
		case TargetType.Self:
			source.commandToExecute.targetList.Add(source);
			break;
		case TargetType.AllEnemies:
			source.commandToExecute.targetList = BattleManager.Instance.GetAllEnemies(source);
			break;
		case TargetType.AllAllies:
			source.commandToExecute.targetList = BattleManager.Instance.GetAllAllies(source);
			break;
		case TargetType.EveryoneElse:
			source.commandToExecute.targetList = BattleManager.Instance.GetEveryoneElse(source);
			break;
		case TargetType.Everyone:
			source.commandToExecute.targetList = BattleManager.Instance.GetEveryone();
			break;
		case TargetType.Random:
			source.commandToExecute.targetList.Add(BattleManager.Instance.GetARandomEnemy(source));
			break;
		}
		return true;
	}
}
