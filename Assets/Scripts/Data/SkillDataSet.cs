using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillDataSet : ScriptableObject 
{
	public List<SkillData> dataSet = new List<SkillData>();
	
	public SkillData GetSkillData(int skillID)
	{
		foreach (SkillData data in dataSet)
		{
			if (data.id == skillID)
				return data;
		}
		Debug.LogError("Asking for an non-exist skill:" + skillID);
		return null;
	}
}


public class SkillData{
/*
 * 技能类。
*/
	public int id;
	public string name;
	public float ATKMultiplier;//技能攻击力倍数
	public float ACCMultiplier;//技能命中率倍数
	public float CRTMultiplier;//技能暴击倍数
	public float preSPDMultiplier;//前摇速度倍数
	public float postSPDMultiplier;//后摇速度倍数
	public int cooldownTurn;//技能冷却回合数
}
