using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillDataSet : ScriptableObject 
{
	public List<SkillData> skillDataSet = new List<SkillData>();
	public List<BuffData> buffDataSet = new List<BuffData>();
	
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
}

[System.Serializable]
public class SkillData{
/*
 * 技能类。
*/
	public int id;
	public string name;
	public SkillType skillType;//技能类型
	public TargetType targetType;//目标类型
	public float ATKMultiplier;//技能攻击力倍数
	public float ACCMultiplier;//技能命中率倍数
	public float CRTMultiplier;//技能暴击倍数
	public float preSPDMultiplier;//前摇速度倍数
	public float postSPDMultiplier;//后摇速度倍数
	public int buffID;//技能对应的BUFFID
	public string description;//技能描述
}

[System.Serializable]
public class BuffData{
	/*
 * BUFF类。
*/
	public int id;
	public string name;
	public string description;
	public int percentage;//触发概率
	public int lastTurns;//持续回合数
	public BuffTrigger trigger;//触发时机
	public string triggerOps;//触发操作
	public string addOps;//上buff操作
	public string removeOps;//下buff操作
}

public enum SkillType{Physical, Magical, Hybrid, IgnoreDefence} 
public enum BuffTrigger{Always,Hit,Behit,Ready,Action} 