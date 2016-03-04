using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillDataSet : ScriptableObject 
{
	public List<SkillData> skillDataSet = new List<SkillData>();
	public List<BuffData> buffDataSet = new List<BuffData>();
	public List<SkillEffectData> effectDataSet = new List<SkillEffectData>();
	
	public SkillData GetSkillData(int skillID)
	{
		foreach (SkillData data in skillDataSet)
		{
			if (data.id == skillID)
				return data;
		}
		Debug.LogError("Asking for an non-exist skill:" + skillID);
		return null;
	}

	public BuffData GetBuffData(int buffID)
	{
		foreach (BuffData data in buffDataSet)
		{
			if (data.id == buffID)
				return data;
		}
		Debug.LogError("Asking for an non-exist buff:" + buffID);
		return null;
	}

	public SkillEffectData GetEffectData(int effectID)
	{
		foreach (SkillEffectData data in effectDataSet)
		{
			if (data.id == effectID)
				return data;
		}
		Debug.LogError("Asking for an non-exist effect:" + effectID);
		return null;
	}
}

[System.Serializable]
public class SkillData{
/*
 * 技能类。
*/
	public int id;
	public string name;
	public string description;//技能描述
	public SkillType skillType;//技能类型
	public TargetType targetType;//目标类型
	public float ATKMultiplier;//技能攻击力倍数
	public float ACCMultiplier;//技能命中率倍数
	public float CRTMultiplier;//技能暴击倍数
	public float preSPDMultiplier;//前摇速度倍数
	public float postSPDMultiplier;//后摇速度倍数or法术消耗
	public float interruptMultiplier;//打断倍数
	public int maxCombo;//最大连击次数or吟唱等级
	public List<int> buffID;
	public List<int> buffPercent;
	public List<int> buffTurns;
	public List<int> effectID;
	public List<int> effectPercent;
}

[System.Serializable]
public class BuffData{
/*
 * BUFF类。
*/
	public int id;
	public string name;
	public string description;
	public BuffTrigger trigger;//触发时机
	public string buffEffect;//效果函数名
	public bool hasIcon;//是否包含图标
}

[System.Serializable]
public class SkillEffectData{
/*
 * SkillEffectData类。
*/
	public int id;
	public string name;
	public string description;
	public EffectTrigger trigger;//触发时机
	public string effectString;//效果函数名
}

