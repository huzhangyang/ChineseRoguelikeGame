using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDataSet : ScriptableObject 
{
	public List<EnemyData> dataSet;
	
	public EnemyData GetEnemyData(int enemyID)
	{
		foreach (EnemyData data in dataSet)
		{
			if (data.enemyID == enemyID)
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
	public int enemyID;
	public EnemyData(int enemyID)
	{
		name = "Enemy";
		this.enemyID = enemyID;
		maxHP = (int)(100 * Random.Range(0.8f, 1.2f));
		maxMP = (int)(100 * Random.Range(0.8f, 1.2f));
		power = (int)(10 * Random.Range(0.8f, 1.2f));
		skill = (int)(10 * Random.Range(0.8f, 1.2f));
		agility = (int)(10 * Random.Range(0.8f, 1.2f));
		toughness = (int)(10 * Random.Range(0.8f, 1.2f));
		luck = (int)(10 * Random.Range(0.8f, 1.2f));
		currentHP = maxHP;
		currentMP = maxMP;
	}
}