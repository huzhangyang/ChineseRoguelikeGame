using UnityEngine;
using System.Collections;

public class DataManager : MonoBehaviour {

/*
数据管理器
*/
	
	private static DataManager s_Instance;
	public DataManager() { s_Instance = this; }
	public static DataManager Instance { get { return s_Instance; } }

	EnemyDatas enemyDatas;

	public void LoadAllDatas()
	{
		enemyDatas = Resources.Load ("GameData/EnemyData", typeof(EnemyDatas)) as EnemyDatas;
		if (enemyDatas == null)
			Debug.LogWarning ("Load EnemyData Failed.");
	}

	public void UnLoadAllDatas()
	{
		enemyDatas = new EnemyDatas ();
	}

	public EnemyDatas GetEnemyDatas()
	{
		return enemyDatas; 
	}
}
