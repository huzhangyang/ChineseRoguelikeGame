using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDataSet : ScriptableObject 
{
	public List<EnemyData> dataSet = new List<EnemyData>();
	
	public EnemyData GetEnemyData(int enemyID)
	{
		foreach (EnemyData data in dataSet)
		{
			if (data.id == enemyID)
				return data;
		}
		Debug.LogError("Asking for an non-exist enemy:" + enemyID);
		return null;
	}
}

[System.Serializable]
public class EnemyData : ObjectData {
/*
 * 怪物数据。除了通用属性外，还有一些怪物独有的属性。
*/
	public bool isBoss;

	public EnemyData Clone()
	{
		return(EnemyData)this.MemberwiseClone();
	}
}