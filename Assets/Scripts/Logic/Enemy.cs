using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : BattleObject {
/*
 * 敌人在战斗中的数据实体与逻辑。
 * */
	public int enemyID;

	EnemyData data = new EnemyData();

	public EnemyData GetData()
	{
		return data;
	}
}
