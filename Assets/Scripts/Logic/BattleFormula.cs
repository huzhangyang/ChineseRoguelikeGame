using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Damage
{
	public float dmg;//伤害值
	public float hit;//命中率[0..100]
	public float crit;//暴击率[0..100]
	public float interrupt;
	public bool isCountered;//是否遭到反击
	public bool isEvaded;//是否遭到闪避
	public bool isGuarded;//是否遭到格挡
	public bool isHit;//是否最终命中
	public bool isCrit;//最终是否暴击
}

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
	public static void CalculateWeaponSkill(BattleObject source, BattleObject target, int skillID)
	{
		//获取必要信息
		SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(skillID);
		WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(source.GetWeapon());

		Damage damagePack = source.damage;
		damagePack = new Damage();
		//计算击出命中、暴击、伤害
		damagePack.dmg = Random.Range(weaponData.basicATKMin, weaponData.basicATKMax + 1) * skillData.ATKMultiplier * BattleAttribute.AttackMulti(source);//伤害值
		damagePack.hit = weaponData.basicACC * skillData.ACCMultiplier + BattleAttribute.ExtraAccuracy(source);//命中率
		damagePack.crit = weaponData.basicCRT * skillData.CRTMultiplier + BattleAttribute.ExtraCrit(source);//暴击率

		//计算真实命中、暴击、伤害
		damagePack.dmg *= (1 - BattleAttribute.DefenceMulti(target));
		damagePack.hit -= BattleAttribute.ExtraEvasion(target);
		damagePack.crit = Mathf.Max(0, damagePack.crit - BattleAttribute.ExtraCritResist(target));
		Debug.Log (source.GetName() + "用" + skillData.name + "攻击" +  target.GetName() + ". Dmg: " + damagePack.dmg + " Hit:" + damagePack.hit + " Crit:" + damagePack.crit);
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
		}
		damagePack.interrupt = weaponData.interrupt * skillData.interruptMultiplier * 100;
		//判断是否命中，是否暴击
		damagePack.isHit = Random.Range(0,101) <= damagePack.hit?true:false;
		damagePack.isCrit = Random.Range(0,101) <= damagePack.crit?true:false;
		//处理最终结果
		if(damagePack.isCountered)//被反击
		{
			OnCounter(source, damagePack);
		}
		else if(damagePack.isGuarded)//被防御[被防御，就不会被暴击]
		{
			OnGuarded(target, damagePack);
		}
		else if(damagePack.isCrit)//暴击
		{
			OnCriticalHit(target, damagePack);
		}
		else if(damagePack.isHit)//正常命中
		{
			OnHit(target, damagePack);
		}
		else//被闪避
		{
			OnMiss(target);
		}
	}

	/*计算武器伤害*/
	public static void CalculateMagicSkill(BattleObject source, BattleObject target, int skillID)
	{
		//获取必要信息
		SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(skillID);
		MagicData magicData = DataManager.Instance.GetItemDataSet().magicDataSet.Find((MagicData _data)=>{return _data.skillID == skillID;});

		Damage damagePack = source.damage;
		damagePack = new Damage();
		//计算击出命中、暴击、伤害
		damagePack.dmg = Random.Range(magicData.basicATKMin, magicData.basicATKMax + 1) * skillData.ATKMultiplier * BattleAttribute.AttackMulti(source);//伤害值
		damagePack.hit = magicData.basicACC * skillData.ACCMultiplier + BattleAttribute.ExtraAccuracy(source);//命中率
		damagePack.crit = magicData.basicCRT * skillData.CRTMultiplier + BattleAttribute.ExtraCrit(source);//暴击率

		//计算真实命中、暴击、伤害
		damagePack.dmg *= (1 - BattleAttribute.DefenceMulti(target));
		damagePack.hit -= BattleAttribute.ExtraEvasion(target);
		damagePack.crit = Mathf.Max(0, damagePack.crit - BattleAttribute.ExtraCritResist(target));
		Debug.Log (source.GetName() + "用" + skillData.name + "攻击" +  target.GetName() + ". Dmg: " + damagePack.dmg + " Hit:" + damagePack.hit + " Crit:" + damagePack.crit);
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
		}
		damagePack.interrupt = magicData.interrupt * skillData.interruptMultiplier * 100;
		//判断是否命中，是否暴击
		damagePack.isHit = Random.Range(0,101) <= damagePack.hit?true:false;
		damagePack.isCrit = Random.Range(0,101) <= damagePack.crit?true:false;
		//处理最终结果
		if(damagePack.isCountered)//被反击
		{
			OnCounter(source, damagePack);
		}
		else if(damagePack.isGuarded)//被防御[被防御，就不会被暴击]
		{
			OnGuarded(target, damagePack);
		}
		else if(damagePack.isCrit)//暴击
		{
			OnCriticalHit(target, damagePack);
		}
		else if(damagePack.isHit)//正常命中
		{
			OnHit(target, damagePack);
		}
		else//被闪避
		{
			OnMiss(target);
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

	/*---------- Internal Operation ----------*/

	private static void OnCounter(BattleObject source, Damage damage)
	{
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name",source.GetName());
		EventManager.Instance.PostEvent(BattleEvent.BattleObjectCounter, args);
		
		OnDamage(source, GetCounterDamage(source), 0);
	}

	private static void OnGuarded(BattleObject target, Damage damage)
	{
		damage.dmg /= 2;
		AudioManager.Instance.PlaySE("guard");
		OnDamage(target, damage.dmg, damage.interrupt);
	}

	private static void OnCriticalHit(BattleObject target, Damage damage)
	{
		damage.dmg *= 2;
		AudioManager.Instance.PlaySE("critical");

		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", target.GetName());
		EventManager.Instance.PostEvent(BattleEvent.BattleObjectCritical, args);

		OnDamage(target, damage.dmg, damage.interrupt);
	}

	private static void OnHit(BattleObject target, Damage damage)
	{
		AudioManager.Instance.PlaySE("hit");
		if(damage.isEvaded)
			OnDamage(target, damage.dmg, 0);
		else
			OnDamage(target, damage.dmg, damage.interrupt);
	}

	private static void OnMiss(BattleObject target)
	{
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", target.GetName());
		EventManager.Instance.PostEvent(BattleEvent.BattleObjectMiss, args);
	}

	private static void OnDamage(BattleObject target, float damage, float interrupt)
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

	private static void OnDead(BattleObject target)
	{
		foreach(Buff buff in target.buffList)
		{
			buff.OnDead();
		}

		if(target.currentHP <= 0)
		{			
			MessageEventArgs args2 = new MessageEventArgs();
			args2.AddMessage("Object", target);
			EventManager.Instance.PostEvent(BattleEvent.BattleObjectDied, args2);
		}
	}
}
