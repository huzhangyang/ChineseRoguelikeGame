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
		//playerData
		playerDataSet = SaveManager.Instance.LoadGame ();
		if(playerDataSet == null)
		{
			Debug.LogWarning ("Load PlayerData Failed.");
			playerDataSet = new PlayerDataSet();
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
		dialogueDataSet = new DialogueDataSet();
		//is save need to be unloaded?
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