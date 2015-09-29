using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleFormula {
/*
 * 战斗计算公式相关
 * */	

	/*计算武器伤害*/
	public static void CalculateWeaponSkill(BattleObject source, BattleObject target, int skillID)
	{
		//获取必要信息
		SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(skillID);
		WeaponData weaponData = DataManager.Instance.GetItemDataSet().GetWeaponData(source.GetWeapon());
		string SEName = "hit";

		//计算击出命中、暴击、伤害
		float hit = source.skill + source.luck / 10.0f + weaponData.basicACC * skillData.ACCMultiplier;//命中率
		float crit = source.skill / 10.0f + source.luck / 10.0f + weaponData.basicCRT * skillData.CRTMultiplier / 100.0f;//暴击率
		float damage = (source.power + weaponData.basicATK) * skillData.ATKMultiplier;//伤害值

		//判断防御反击
		if(target.commandToExecute.commandType == CommandType.Defence)
		{
			MessageEventArgs args = new MessageEventArgs();
			args.AddMessage("Name",source.GetName());
			EventManager.Instance.PostEvent(EventDefine.BattleObjectCounter, args);
			
			InflictDamage(source, Random.Range(0,100));//TODO 改进反击算法
			return;
		}

		//计算真实命中、暴击、伤害
		hit -= (target.skill + target.luck / 10.0f);
		crit -= (target.skill / 10.0f + target.luck / 10.0f);
		damage *= (1 - target.toughness / 250);

		//加入防御效果
		if(target.isEvading)
		{
			hit -= 50;
		}
		if(target.isGuarding)
		{
			crit = 0;
			damage /= 2;
			SEName = "guard";
		}
		//判断是否命中，是否暴击
		bool isHit = Random.Range(0,101) <= hit?true:false;
		bool isCrit = Random.Range(0,101) <= crit?true:false;

		if(isHit)
		{		
			if(isCrit)
			{
				damage *= 2;
				SEName = "critical";
				
				MessageEventArgs args = new MessageEventArgs();
				args.AddMessage("Name", target.GetName());
				EventManager.Instance.PostEvent(EventDefine.BattleObjectCritical, args);
			}
			AudioManager.Instance.PlaySE(SEName);
			InflictDamage(target, (int)damage);
		}
		else
		{
			MessageEventArgs args = new MessageEventArgs();
			args.AddMessage("Name", target.GetName());
			EventManager.Instance.PostEvent(EventDefine.BattleObjectMiss, args);
		}
	}

	/*计算武器伤害*/
	public static void CalculateMagicSkill(BattleObject source, BattleObject target, int skillID)
	{
		//获取必要信息
		SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(skillID);
		MagicData magicData = DataManager.Instance.GetItemDataSet().magicDataSet.Find((MagicData _data)=>{return _data.skillID == skillID;});
		string SEName = "hit";
		
		//计算击出命中、暴击、伤害
		float hit = source.skill + source.luck / 10.0f + magicData.basicACC * skillData.ACCMultiplier;//命中率
		float crit = source.skill / 10.0f + source.luck / 10.0f + magicData.basicCRT * skillData.CRTMultiplier / 100.0f;//暴击率
		float damage = (source.power + magicData.basicATK) * skillData.ATKMultiplier;//伤害值
		
		//判断防御反击
		if(target.commandToExecute.commandType == CommandType.Defence)
		{
			MessageEventArgs args = new MessageEventArgs();
			args.AddMessage("Name",source.GetName());
			EventManager.Instance.PostEvent(EventDefine.BattleObjectCounter, args);
			
			InflictDamage(source, Random.Range(0,100));//TODO 改进反击算法
			return;
		}
		
		//计算真实命中、暴击、伤害
		hit -= (target.skill + target.luck / 10.0f);		
		crit -= (target.skill / 10.0f + target.luck / 10.0f);		
		damage *= (1 - target.insight / 250);
		
		//加入防御效果
		if(target.isEvading)
		{
			hit -= 50;
		}
		if(target.isGuarding)
		{
			crit = 0;
			damage /= 2;
			SEName = "guard";
		}
		//判断是否命中，是否暴击
		bool isHit = Random.Range(0,101) <= hit?true:false;
		bool isCrit = Random.Range(0,101) <= crit?true:false;
		
		if(isHit)
		{		
			if(isCrit)
			{
				damage *= 2;
				SEName = "critical";
				
				MessageEventArgs args = new MessageEventArgs();
				args.AddMessage("Name", target.GetName());
				EventManager.Instance.PostEvent(EventDefine.BattleObjectCritical, args);
			}
			AudioManager.Instance.PlaySE(SEName);
			InflictDamage(target, (int)damage);
		}
		else
		{
			MessageEventArgs args = new MessageEventArgs();
			args.AddMessage("Name", target.GetName());
			EventManager.Instance.PostEvent(EventDefine.BattleObjectMiss, args);
		}
	}

	public static void InflictDamage(BattleObject target, int damage)
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
	
	public void Heal(BattleObject target, int amount)
	{
		if(amount < 0) amount = 0;
		target.currentHP += amount;
		
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", target.GetName());
		args.AddMessage("Amount", amount.ToString());
		args.AddMessage("CurrentHP", target.currentHP.ToString());
		EventManager.Instance.PostEvent(EventDefine.BattleObjectHeal, args);
	}	
}
