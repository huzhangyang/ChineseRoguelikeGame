using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int enemyID;
	EnemyData data = new EnemyData();

	public EnemyData GetData()
	{
		return data;
	}
}
