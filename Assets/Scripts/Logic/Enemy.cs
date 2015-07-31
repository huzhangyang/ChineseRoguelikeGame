using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : BattleObject {
/*
 * 敌人在战斗中的数据实体与逻辑。
 * */
	public int enemyID;

	void Start()
	{
		data = new EnemyData(enemyID);
	}

	public EnemyData GetData()
	{
		return (EnemyData)data;
	}
}
