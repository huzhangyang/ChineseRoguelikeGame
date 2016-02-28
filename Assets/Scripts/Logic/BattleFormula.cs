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
		Damage damagePack = source.damage = target.damageTaken = new Damage(source, target, skillID, isWeaponDamage);

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

		SkillHelper.CheckSkillEffect (EffectTrigger.OnHit, source);//检查命中前生效的特效

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
			damagePack.dmg = Random.Range(damagePack.minDmg, damagePack.maxDmg) * target.GetSkillTypeMulti(skillData.skillType);
			damagePack.isHit = Random.Range(0,101) <= damagePack.hit?true:false;
			damagePack.isCrit = Random.Range(0,101) <= damagePack.crit?true:false;
			damagePack.isHit = (damagePack.forceHit|| damagePack.target == damagePack.source) ? true : damagePack.forceMiss ? false : damagePack.isHit;
			damagePack.isCrit = damagePack.forceCrit ? true : damagePack.isCrit;

			SkillHelper.CheckBuff (BuffTrigger.Behit, target);//检查命中前生效的特效

			damagePack.TakeEffect();

			if(damagePack.isCountered || damagePack.target.isDead || (!damagePack.isHit && damagePack.stopComboOnMiss))
			{
				break;
			}
		}
	}
	
	public static void Heal(BattleObject target, int amount)
	{
		if(amount < 0) amount = 0;
		target.currentHP += amount;

		target.GetComponent<BattleObjectUIEvent>().OnHeal();

		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", target.GetName());
		args.AddMessage("Amount", amount);
		args.AddMessage("CurrentHP", target.currentHP);
		EventManager.Instance.PostEvent(BattleEvent.BattleObjectHeal, args);
	}

	public static void OnDamage(BattleObject target)
	{
		target.currentHP -= Mathf.RoundToInt(target.damageTaken.dmg);
		//timeline interrupt
		target.timelinePosition -= Mathf.RoundToInt(target.damageTaken.interrupt);
		if(target.battleStatus == BattleStatus.Action && target.timelinePosition < GlobalDataStructure.BATTLE_TIMELINE_READY)
		{
			target.battleStatus = BattleStatus.Prepare;
			target.timelinePosition = 0;
			//BattleManager.Instance.RemoveFromCommandQueue(target.commandToExecute);
			MessageEventArgs args = new MessageEventArgs();
			args.AddMessage("Name", target.GetName());
			EventManager.Instance.PostEvent(BattleEvent.BattleObjectInterrupted, args);
		}
		target.GetComponent<BattleObjectUIEvent>().OnDamage();
		//send message
		MessageEventArgs args2 = new MessageEventArgs();
		args2.AddMessage("Name", target.GetName());
		args2.AddMessage("Damage", Mathf.RoundToInt(target.damageTaken.dmg));
		EventManager.Instance.PostEvent(BattleEvent.BattleObjectHurt, args2);
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
			target.buffFrozenTime = 1;
			target.isDead = true;

			MessageEventArgs args2 = new MessageEventArgs();
			args2.AddMessage("Object", target);
			EventManager.Instance.PostEvent(BattleEvent.BattleObjectDied, args2);
		}
	}
}
