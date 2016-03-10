using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDataSet : ScriptableObject 
{
	public List<EnemyData> enemyDataSet = new List<EnemyData>();
	public List<AIData> aiDataSet = new List<AIData>();
	
	public EnemyData GetEnemyData(int enemyID)
	{
		foreach (EnemyData data in enemyDataSet)
		{
			if (data.id == enemyID)
				return data;
		}
		Debug.LogError("Asking for an non-exist enemy:" + enemyID);
		return null;
	}

	public AIData GetAIData(int aiID)
	{
		foreach (AIData data in aiDataSet)
		{
			if (data.id == aiID)
				return data;
		}
		Debug.LogError("Asking for an non-exist ai:" + aiID);
		return null;
	}
}

[System.Serializable]
public class EnemyData : ObjectData {
/*
 * 怪物数据。除了通用属性外，还有一些怪物独有的属性。
*/
	public bool isBoss;
	public int imageID;
	public int aiID;

	public EnemyData Clone()
	{
		OnDeSerialize();
		return(EnemyData)this.MemberwiseClone();
	}
}

[System.Serializable]
public class AIData {
/*
 * 怪物行为逻辑数据。
*/
	public int id;
	public string name;
	public int attackFrequency;
	public int defenceFrequency;
	public int itemFrequency;
	public int strategyFrequency;
	public int escapeThreshold;
	public bool useGuard;
}