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
		return Mathf.RoundToInt((bo.power + bo.skill) * BattleAttribute.AttackMulti(bo));
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
			damagePack.minDmg = weaponData.basicATKMin * skillData.ATKMultiplier * BattleAttribute.AttackMulti(source);//最小伤害值
			damagePack.maxDmg = weaponData.basicATKMax * skillData.ATKMultiplier * BattleAttribute.AttackMulti(source);//最大伤害值
			damagePack.hit = weaponData.basicACC * skillData.ACCMultiplier + BattleAttribute.ExtraAccuracy(source);//命中率
			damagePack.crit = weaponData.basicCRT * skillData.CRTMultiplier + BattleAttribute.ExtraCrit(source);//暴击率
			damagePack.interrupt = weaponData.interrupt * skillData.interruptMultiplier * 100;//打断
		}
		else
		{
			MagicData magicData = DataManager.Instance.GetItemDataSet().GetMagicDataBySkillID(skillID);
			damagePack.minDmg = magicData.basicATKMin * skillData.ATKMultiplier * BattleAttribute.AttackMulti(source);//最小伤害值
			damagePack.maxDmg = magicData.basicATKMax * skillData.ATKMultiplier * BattleAttribute.AttackMulti(source);//最大伤害值
			damagePack.hit = magicData.basicACC * skillData.ACCMultiplier + BattleAttribute.ExtraAccuracy(source);//命中率
			damagePack.crit = magicData.basicCRT * skillData.CRTMultiplier + BattleAttribute.ExtraCrit(source);//暴击率
			damagePack.interrupt = magicData.interrupt * skillData.interruptMultiplier * 100;
		}

		SkillHelper.CheckSkillEffect (EffectTrigger.BeforeHit, source);//检查命中前生效的特效

		//计算真实命中、暴击、伤害
		if(!damagePack.ignoreArmor)
		{
			damagePack.minDmg *= (1 - BattleAttribute.DefenceMulti(target));
			damagePack.maxDmg *= (1 - BattleAttribute.DefenceMulti(target));
		}
		damagePack.hit -= BattleAttribute.ExtraEvasion(target);
		damagePack.crit = Mathf.Max(0, damagePack.crit - BattleAttribute.ExtraCritResist(target));

		if(!damagePack.ignoreDefence)
		{
			//判断防御反击
			if(target.commandToExecute.commandType == CommandType.Defence)
			{
				damagePack.isCountered = true;
			}
			//加入防御效果
			if(target.isEvading)
			{
				damagePack.isEvaded = true;
				damagePack.hit /= 2;
			}
			if(target.isGuarding)
			{
				damagePack.isGuarded = true;
				damagePack.minDmg /= 2;
				damagePack.maxDmg /= 2;
				damagePack.interrupt = 0;
			}
		}

		for(int i = 0; i < damagePack.combo; i++)
		{
			//判断是否命中，是否暴击,随机实际伤害值
			damagePack.dmg = Random.Range(Mathf.RoundToInt(damagePack.minDmg), Mathf.RoundToInt(damagePack.maxDmg) + 1);
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

	public static void OnDamage(BattleObject target, Damage damage)
	{
		target.currentHP -= Mathf.RoundToInt(damage.dmg);
		//timeline interrupt
		target.timelinePosition -= Mathf.RoundToInt(damage.interrupt);
		if(target.timelinePosition < GlobalDataStructure.BATTLE_TIMELINE_READY)
			target.battleStatus = BattleStatus.Prepare;
		//send message
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", target.GetName());
		args.AddMessage("Damage", Mathf.RoundToInt(damage.dmg));
		EventManager.Instance.PostEvent(BattleEvent.BattleObjectHurt, args);
		//calculate die event
		if(target.currentHP <= 0)
		{			
			OnDead(target);
		}
	}

	public static void OnCounterDamage(BattleObject source, BattleObject target)
	{
		target.currentHP -= GetCounterDamage(source);
		//send message
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", target.GetName());
		args.AddMessage("Damage", GetCounterDamage(source));
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
