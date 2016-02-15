using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleFormula {
/*
 * 战斗计算公式相关
 * */	

	//计算防御反击伤害
	public static int GetCounterDamage(BattleObject bo)
	{
		return (int)Mathf.Round((bo.power + bo.skill) * BattleAttribute.AttackMulti(bo));
	}

	/*计算武器伤害*/
	public static void CalculateSkill(BattleObject source, BattleObject target, int skillID, bool isWeaponDamage)
	{
		//获取必要信息
		SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(skillID);
		Damage damagePack = source.damage = new Damage(source, target, skillID, isWeaponDamage);

		//计算击出命中、暴击、伤害
		if(isWeaponDamage)
		{
			WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(source.GetWeapon());
			damagePack.dmg = Random.Range(weaponData.basicATKMin, weaponData.basicATKMax + 1) * skillData.ATKMultiplier * BattleAttribute.AttackMulti(source);//伤害值
			damagePack.hit = weaponData.basicACC * skillData.ACCMultiplier + BattleAttribute.ExtraAccuracy(source);//命中率
			damagePack.crit = weaponData.basicCRT * skillData.CRTMultiplier + BattleAttribute.ExtraCrit(source);//暴击率
			damagePack.interrupt = weaponData.interrupt * skillData.interruptMultiplier * 100;//打断
		}
		else
		{
			MagicData magicData = DataManager.Instance.GetItemDataSet().GetMagicDataBySkillID(skillID);
			damagePack.dmg = Random.Range(magicData.basicATKMin, magicData.basicATKMax + 1) * skillData.ATKMultiplier * BattleAttribute.AttackMulti(source);//伤害值
			damagePack.hit = magicData.basicACC * skillData.ACCMultiplier + BattleAttribute.ExtraAccuracy(source);//命中率
			damagePack.crit = magicData.basicCRT * skillData.CRTMultiplier + BattleAttribute.ExtraCrit(source);//暴击率
			damagePack.interrupt = magicData.interrupt * skillData.interruptMultiplier * 100;
		}

		SkillHelper.CheckSkillEffect (EffectTrigger.BeforeHit, source);//检查命中前生效的特效

		//计算真实命中、暴击、伤害
		damagePack.dmg *= (1 - BattleAttribute.DefenceMulti(target));
		damagePack.hit -= BattleAttribute.ExtraEvasion(target);
		damagePack.crit = Mathf.Max(0, damagePack.crit - BattleAttribute.ExtraCritResist(target));

		//判断防御反击
		if(target.commandToExecute.commandType == CommandType.Defence)
		{
			damagePack.isCountered = true;
		}

		if(!damagePack.ignoreDefence)
		{
			//加入防御效果
			if(target.isEvading)
			{
				damagePack.isEvaded = true;
				damagePack.hit /= 2;
			}
			if(target.isGuarding)
			{
				damagePack.isGuarded = true;
				damagePack.dmg /= 2;
				damagePack.interrupt = 0;
			}
		}

		//判断是否命中，是否暴击
		damagePack.isHit = Random.Range(0,101) <= damagePack.hit?true:false;
		damagePack.isCrit = Random.Range(0,101) <= damagePack.crit?true:false;
		damagePack.isHit = damagePack.forceHit ? true : damagePack.forceMiss ? false : damagePack.isHit;
		damagePack.isCrit = damagePack.forceCrit ? true : damagePack.isCrit;
		
		SkillHelper.CheckSkillEffect (EffectTrigger.OnDamage, source);//检查结算中生效的特效
		//damagePack.Log ();

		//处理最终结果
		if(damagePack.isCountered)//被反击
		{
			damagePack.OnCounter();
		}
		else if(!damagePack.isHit)//被闪避
		{
			damagePack.OnMiss();
		}
		else if(damagePack.isGuarded)//被防御[被防御，就不会被暴击]
		{
			damagePack.OnGuarded();
		}
		else if(damagePack.isCrit)//暴击
		{
			damagePack.OnCriticalHit();
		}
		else//正常命中
		{
			damagePack.OnHit();
		}
	}
	
	public static void Heal(BattleObject target, int amount)
	{
		if(amount < 0) amount = 0;
		target.currentHP += amount;
		
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", target.GetName());
		args.AddMessage("Amount", amount);
		args.AddMessage("CurrentHP", target.currentHP);
		EventManager.Instance.PostEvent(BattleEvent.BattleObjectHeal, args);
	}

	public static void OnDamage(BattleObject target, float damage, float interrupt)
	{
		target.currentHP -= (int)Mathf.Round(damage);
		//timeline interrupt
		target.timelinePosition -= (int)Mathf.Round(interrupt);
		if(target.timelinePosition < GlobalDataStructure.BATTLE_TIMELINE_READY)
			target.battleStatus = BattleStatus.Prepare;
		//send message
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", target.GetName());
		args.AddMessage("Damage", (int)Mathf.Round(damage));
		EventManager.Instance.PostEvent(BattleEvent.BattleObjectHurt, args);
		//calculate die event
		if(target.currentHP <= 0)
		{			
			OnDead(target);
		}
	}

	/*---------- Internal Operation ----------*/
	private static void OnDead(BattleObject target)
	{
		SkillHelper.CheckBuff (BuffTrigger.Dead, target);

		if(target.currentHP <= 0)
		{			
			MessageEventArgs args2 = new MessageEventArgs();
			args2.AddMessage("Object", target);
			EventManager.Instance.PostEvent(BattleEvent.BattleObjectDied, args2);
		}
	}
}
