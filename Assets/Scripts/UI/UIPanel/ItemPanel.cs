using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemPanel : MonoBehaviour {

	public Transform itemList;
	private PlayerData playerData;

	void Start()
	{
		SetData();
	}

	void OnEnable()
	{
		EventManager.Instance.RegisterEvent(UIEvent.OnSwitchLeader, OnSwitchLeader);
	}

	void OnDisable()
	{
		EventManager.Instance.UnRegisterEvent(UIEvent.OnSwitchLeader, OnSwitchLeader);
	}

	void OnSwitchLeader(MessageEventArgs args)
	{
		SetData();
	}

	void SetData()
	{
		playerData = DataManager.Instance.GetPlayerDataSet().GetPlayerData(DataManager.Instance.GetConfigData().currentLeaderID);
		InitItemButtons();
	}

	private void InitItemButtons()
	{
		for(int i = 0; i < itemList.childCount; i++)
		{
			GameObject.Destroy(itemList.GetChild(i).gameObject);
		}
		foreach(var itemSlot in playerData.GetItemDict())
		{
			GameObject itemButton = Instantiate(Resources.Load(GlobalDataStructure.PATH_UIPREFAB_COMMON + "ItemButton")) as GameObject;
			itemButton.transform.SetParent(itemList, false);
			itemButton.GetComponent<ItemButtonUIEvent>().Init(itemSlot.Key, itemSlot.Value);
		}
	}
}
