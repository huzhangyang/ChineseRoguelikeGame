using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Damage
{
/*
 * 伤害
 * */	

	public BattleObject source;//伤害来源
	public BattleObject target;//伤害对象
	public int skillID;//造成伤害的技能ID（如果有）
	public bool isWeaponDamage;//伤害是通过武器造成的

	public float dmg;//伤害值
	public float hit;//命中率[0..100]
	public float crit;//暴击率[0..100]
	public float interrupt;
	public bool isCountered;//是否遭到反击
	public bool isEvaded;//是否遭到闪避
	public bool isGuarded;//是否遭到格挡
	public bool isHit;//是否最终命中
	public bool isCrit;//最终是否暴击
	
	public bool forceHit;//强制命中
	public bool forceMiss;//强制不中（优先级低于强制命中）
	public bool forceCrit;//强制暴击
	public bool ignoreDefence;//无视防御
	public bool ignoreArmor;//无视护甲
	
	public Damage(BattleObject source, BattleObject target, int skillID, bool isWeaponDamage)
	{
		this.source = source;
		this.target = target;
		this.skillID = skillID;
		this.isWeaponDamage = isWeaponDamage;
	}
	
	public void Log()
	{
		SkillData skillData = DataManager.Instance.GetSkillDataSet().GetSkillData(skillID);
		string log = string.Format ("{0}用{1}攻击{2}.Dmg:{3} Hit:{4} Crit:{5}",
		                            source.GetName(), skillData.name, target.GetName(), dmg, hit, crit);
		Debug.Log (log);
	}

	public void OnCounter()
	{
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name",target.GetName());
		EventManager.Instance.PostEvent(BattleEvent.BattleObjectCounter, args);
		
		BattleFormula.OnDamage(source, BattleFormula.GetCounterDamage(source), 0);

	}
	
	public void OnGuarded()
	{
		AudioManager.Instance.PlaySE("guard");
		BattleFormula.OnDamage(target, this.dmg, this.interrupt);
		SkillHelper.CheckSkillEffect (EffectTrigger.AfterDamage, source);//检查伤害后生效的特效
		SkillHelper.CheckBuffAdd (source);//检查附带的BUFF是否命中
	}
	
	public void OnCriticalHit()
	{
		this.dmg *= 2;
		AudioManager.Instance.PlaySE("critical");
		
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", target.GetName());
		EventManager.Instance.PostEvent(BattleEvent.BattleObjectCritical, args);
		
		BattleFormula.OnDamage(target, this.dmg, this.interrupt);
		SkillHelper.CheckSkillEffect (EffectTrigger.AfterDamage, source);//检查伤害后生效的特效
		SkillHelper.CheckBuffAdd (source);//检查附带的BUFF是否命中
	}
	
	public void OnHit()
	{
		AudioManager.Instance.PlaySE("hit");

		BattleFormula.OnDamage(target, this.dmg, this.interrupt);
		SkillHelper.CheckSkillEffect (EffectTrigger.AfterDamage, source);//检查伤害后生效的特效
		SkillHelper.CheckBuffAdd (source);//检查附带的BUFF是否命中
	}
	
	public void OnMiss()
	{
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("Name", target.GetName());
		EventManager.Instance.PostEvent(BattleEvent.BattleObjectMiss, args);
	}
}