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
	DialogueDataSet dialogueDataSet;
	ItemDataSet itemDataSet;
	SkillDataSet skillDataSet;
	PlayerDataSet playerDataSet;
	ConfigData configData;

	public void LoadAllData()
	{
		//enemyData
		enemyDataSet = Resources.Load (GlobalDataStructure.PATH_GAMEDATA + "EnemyData", typeof(EnemyDataSet)) as EnemyDataSet;
		if (enemyDataSet == null)
		{
			Debug.LogWarning ("Load EnemyData Failed.");
			enemyDataSet = new EnemyDataSet();
		}
		//dialogueData
		dialogueDataSet = Resources.Load (GlobalDataStructure.PATH_GAMEDATA + "DialogueData", typeof(DialogueDataSet)) as DialogueDataSet;
		if (dialogueDataSet == null)
		{
			Debug.LogWarning ("Load DialogueData Failed.");
			dialogueDataSet = new DialogueDataSet();
		}
		//itemData
		itemDataSet = Resources.Load (GlobalDataStructure.PATH_GAMEDATA + "ItemData", typeof(ItemDataSet)) as ItemDataSet;
		if (itemDataSet == null)
		{
			Debug.LogWarning ("Load ItemData Failed.");
			itemDataSet = new ItemDataSet();
		}
		//skillData
		skillDataSet = Resources.Load (GlobalDataStructure.PATH_GAMEDATA + "SkillData", typeof(SkillDataSet)) as SkillDataSet;
		if (skillDataSet == null)
		{
			Debug.LogWarning ("Load SkillData Failed.");
			skillDataSet = new SkillDataSet();
		}
	}

	public void LoadSave()
	{
		playerDataSet = SaveManager.Instance.LoadGame ();
		if(playerDataSet == null)
		{
			Debug.LogWarning ("Load PlayerData Failed. Creating New Instead.");
			CreateNewSave();
		}
	}

	public void CreateNewSave()
	{
		playerDataSet = new PlayerDataSet();
		PlayerData data0 = new PlayerData();
		data0.InitWithID(0);
		PlayerData data1 = new PlayerData();
		data1.InitWithID(1);
		playerDataSet.dataSet.Add(data0);
		playerDataSet.dataSet.Add(data1);
		SaveManager.Instance.SaveGame();
	}

	public void LoadConfig()
	{
		configData = SaveManager.Instance.LoadConfig ();
		if(configData == null)
		{
			Debug.LogWarning ("Load ConfigData Failed. Creating New Instead.");
			CreateNewConfig();
		}
	}

	public void CreateNewConfig()
	{
		configData = new ConfigData();
		SaveManager.Instance.SaveConfig();
	}

	public void UnLoadAllData()
	{
		enemyDataSet = new EnemyDataSet ();
		dialogueDataSet = new DialogueDataSet();
		enemyDataSet = new EnemyDataSet();
		playerDataSet = new PlayerDataSet();
		itemDataSet = new ItemDataSet();
		skillDataSet = new SkillDataSet();
	}

	public EnemyDataSet GetEnemyDataSet()
	{
		return enemyDataSet; 
	}

	public DialogueDataSet GetDialogueDataSet()
	{
		return dialogueDataSet; 
	}

	public ItemDataSet GetItemDataSet()
	{
		return itemDataSet; 
	}
	
	public SkillDataSet GetSkillDataSet()
	{
		return skillDataSet; 
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