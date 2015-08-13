using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager : MonoBehaviour {

/*
数据管理器
*/
	
	private static DataManager s_Instance;
	public DataManager() { s_Instance = this; }
	public static DataManager Instance { get { return s_Instance; } }

	EnemyDataSet enemyDataSet;
	PlayerDataSet playerDataSet;
	DialogueDataSet dialogueDataSet;
	ConfigData configData;

	public void LoadAllData()
	{
		//enemyData
		enemyDataSet = Resources.Load (GlobalDataStructure.PATH_GAMEDATA + "EnemyData", typeof(EnemyDataSet)) as EnemyDataSet;
		if (enemyDataSet == null)
		{
			Debug.LogWarning ("Load EnemyData Failed.");
			enemyDataSet = new EnemyDataSet();
			enemyDataSet.dataSet = new List<EnemyData>();
		}
		//dialogueData
		dialogueDataSet = Resources.Load (GlobalDataStructure.PATH_GAMEDATA + "DialogueData", typeof(DialogueDataSet)) as DialogueDataSet;
		if (dialogueDataSet == null)
		{
			Debug.LogWarning ("Load DialogueData Failed.");
			dialogueDataSet = new DialogueDataSet();
			dialogueDataSet.dataSet = new List<DialogueData>();
		}
		//playerData
		playerDataSet = SaveManager.Instance.LoadGame ();
		if(playerDataSet == null)
		{
			Debug.LogWarning ("Load PlayerData Failed.");
			playerDataSet = new PlayerDataSet();
			playerDataSet.dataSet = new List<PlayerData>();
			playerDataSet.dataSet.Add(new PlayerData(0));
			playerDataSet.dataSet.Add(new PlayerData(1));
		}
		//configData
		configData = SaveManager.Instance.LoadConfig ();
		if(configData == null)
		{
			Debug.LogWarning ("Load ConfigData Failed.");
			configData = new ConfigData();
		}
	}

	public void UnLoadAllData()
	{
		enemyDataSet = new EnemyDataSet ();
	}

	public EnemyDataSet GetEnemyDataSet()
	{
		return enemyDataSet; 
	}

	public DialogueDataSet GetDialogueDataSet()
	{
		return dialogueDataSet; 
	}

	public PlayerDataSet GetPlayerDataSet()
	{
		return playerDataSet;
	}

	public ConfigData GetConfigData()
	{
		return configData;
	}

}