using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Damage
{
	public float dmg;//伤害值
	public float hit;//命中率[0..100]
	public float crit;//暴击率[0..100]
	public bool isCountered;//是否遭到反击
	public bool isGuarded;//是否遭到格挡
	public bool isHit;//是否最终命中
	public bool isCrit;//最终是否暴击
}

public class BattleFormula {
/*
 * 战斗计算公式相关
 * */
	public static int GetMaxHP(BattleObject bo)
	{
		if(bo.GetBattleType() == BattleType.Physical)
			return (int)(Mathf.Pow(bo.stamina, 1.5f) * 3);
		else
			return (int)(Mathf.Pow(bo.stamina, 1.4f) * 4);
	}

	public static int GetTimelineStep(BattleObject bo)
	{
		return (int)(Mathf.Log10(bo.agility) * 80);
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
		damagePack.dmg = (source.power + weaponData.basicATK) * skillData.ATKMultiplier;//伤害值
		damagePack.hit = source.skill + source.luck / 10.0f + weaponData.basicACC * skillData.ACCMultiplier;//命中率
		damagePack.crit = source.skill / 10.0f + source.luck / 10.0f + weaponData.basicCRT * skillData.CRTMultiplier / 100.0f;//暴击率
		//计算真实命中、暴击、伤害
		damagePack.dmg *= (1 - target.toughness / 250);
		damagePack.hit -= (target.skill + target.luck / 10.0f);
		damagePack.crit -= (target.skill / 10.0f + target.luck / 10.0f);
		//判断防御反击
		if(target.commandToExecute.commandType == CommandType.Defence)
		{
			damagePack.isCountered = true;
		}
		//加入防御效果
		if(target.isEvading)
		{
			damagePack.hit -= 50;
		}
		if(target.isGuarding)
		{
			damagePack.isGuarded = true;
		}
		//判断是否命中，是否暴击
		damagePack.isHit = Random.Range(0,101) <= damagePack.hit?true:false;
		damagePack.isCrit = Random.Range(0,101) <= damagePack.crit?true:false;
		//处理buff
		foreach(Buff buff in source.buffList)
		{
			buff.OnHit();
		}
		foreach(Buff buff in target.buffList)
		{
			buff.OnBeHit();
		}
		//处理最终结果
		if(damagePack.isCountered)//被反击
		{
			OnCounter(source);
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
		damagePack.dmg = (source.power + magicData.basicATK) * skillData.ATKMultiplier;//伤害值
		damagePack.hit = source.skill + source.luck / 10.0f + magicData.basicACC * skillData.ACCMultiplier;//命中率
		damagePack.crit = source.skill / 10.0f + source.luck / 10.0f + magicData.basicCRT * skillData.CRTMultiplier / 100.0f;//暴击率
		//计算真实命中、暴击、伤害
		damagePack.dmg *= (1 - target.insight / 250);
		damagePack.hit -= (target.skill + target.luck / 10.0f);
		damagePack.crit -= (target.skill / 10.0f + target.luck / 10.0f);
		//判断防御反击
		if(target.commandToExecute.commandType == CommandType.Defence)
		{
			damagePack.isCountered = true;
		}
		//加入防御效果
		if(target.isEvading)
		{
			damagePack.hit -= 50;
		}
		if(target.isGuarding)
		{
			damagePack.isGuarded = true;
		}
		//判断是否命中，是否暴击
		damagePack.isHit = Random.Range(0,101) <= damagePack.hit?true:false;
		damagePack.isCrit = Random.Range(0,101) <= damagePack.crit?true:false;
		//处理buff
		foreach(Buff buff in source.buffList)
		{
			buff.OnHit();
		}
		foreach(Buff buff in target.buffList)
		{
			buff.OnBeHit();
		}
		//处理最终结果
		if(damagePack.isCountered)//被反击
		{
			OnCounter(source);
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
		args.AddMessage("Amount", amount.ToString());
		args.AddMessage("CurrentHP", target.currentHP.ToString());
		EventManager.Instance.PostEvent(EventDefine.BattleObjectHeal, args);
	}

	/*---------- Internal Operation ----------*/

	private static void OnCounter(BattleObject source)
	{
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name",source.GetName());
		EventManager.Instance.PostEvent(EventDefine.BattleObjectCounter, args);
		
		OnDamage(source, Random.Range(0,100));//TODO 改进反击算法
	}

	private static void OnGuarded(BattleObject target, Damage damage)
	{
		damage.dmg /= 2;
		AudioManager.Instance.PlaySE("guard");
		OnDamage(target, (int)damage.dmg);
	}

	private static void OnCriticalHit(BattleObject target, Damage damage)
	{
		damage.dmg *= 2;
		AudioManager.Instance.PlaySE("critical");

		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", target.GetName());
		EventManager.Instance.PostEvent(EventDefine.BattleObjectCritical, args);

		OnDamage(target, (int)damage.dmg);
	}

	private static void OnHit(BattleObject target, Damage damage)
	{
		AudioManager.Instance.PlaySE("hit");
		OnDamage(target, (int)damage.dmg);
	}

	private static void OnMiss(BattleObject target)
	{
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", target.GetName());
		EventManager.Instance.PostEvent(EventDefine.BattleObjectMiss, args);
	}

	private static void OnDamage(BattleObject target, int damage)
	{
		target.currentHP -= damage;
		target.isPaused = true;// so that timeline adjust is smooth
		//timeline drawback
		if(target.battleStatus == BattleStatus.Action)
			target.timelinePosition -= damage * 10000 / target.maxHP;
		else
			target.timelinePosition -= damage * 5000 / target.maxHP;
		//send message
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", target.GetName());
		args.AddMessage("Damage", damage.ToString());
		EventManager.Instance.PostEvent(EventDefine.BattleObjectHurt, args);
		//calculate die event
		if(target.currentHP <= 0)
		{
			target.isDied = true;
			if(target is Enemy)
				BattleLogic.enemys.Remove((Enemy)target);
			else
				BattleLogic.players.Remove((Player)target);
			
			MessageEventArgs args2 = new MessageEventArgs();
			args2.AddMessage("Name", target.GetName());
			EventManager.Instance.PostEvent(EventDefine.BattleObjectDied, args2);
		}
	}
}
