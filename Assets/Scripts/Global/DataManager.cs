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

	EnemyDatas enemyDatas;
	PlayerDatas playerDatas;
	DialogueDatas dialogueDatas;
	//ConfigData configData;

	public void LoadAllDatas()
	{
		//enemyData
		enemyDatas = Resources.Load ("GameData/EnemyData", typeof(EnemyDatas)) as EnemyDatas;
		if (enemyDatas == null)
		{
			Debug.LogWarning ("Load EnemyData Failed.");
			enemyDatas = new EnemyDatas();
			enemyDatas.datas = new List<EnemyData>();
		}
		//dialogueData
		dialogueDatas = Resources.Load ("GameData/DialogueData", typeof(DialogueDatas)) as DialogueDatas;
		if (dialogueDatas == null)
		{
			Debug.LogWarning ("Load DialogueData Failed.");
			dialogueDatas = new DialogueDatas();
			dialogueDatas.datas = new List<DialogueData>();
		}
		//playerData
		SaveManager.Instance.LoadGame ();
		if(playerDatas == null)
		{
			Debug.LogWarning ("Load PlayerData Failed.");
			playerDatas = new PlayerDatas();
			playerDatas.datas = new List<PlayerData>();
			playerDatas.datas.Add(new PlayerData(0));
			playerDatas.datas.Add(new PlayerData(1));
		}
		//configData
		/*SaveManager.Instance.LoadConfig ();
		if(configData == null)
		{
			Debug.LogWarning ("Load ConfigData Failed.");
			configData = new ConfigData();
		}*/
	}

	public void UnLoadAllDatas()
	{
		enemyDatas = new EnemyDatas ();
	}

	public EnemyDatas GetEnemyDatas()
	{
		return enemyDatas; 
	}

	public DialogueDatas GetDialogueDatas()
	{
		return dialogueDatas; 
	}

	public PlayerDatas GetPlayerDatas()
	{
		return playerDatas;
	}

	public void SetPlayerDatas(PlayerData[] playerDataArray)
	{
		playerDatas = new PlayerDatas ();
		playerDatas.datas = new List<PlayerData> (playerDataArray);
	}
}
