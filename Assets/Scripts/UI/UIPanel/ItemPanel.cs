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
		playerData.AcquireItem(1,3);
		InitItemButtons();
	}

	private void InitItemButtons()
	{
		for(int i = 0; i < itemList.childCount; i++)
		{
			GameObject.Destroy(itemList.GetChild(i).gameObject);
		}
		for(int i = 0; i < playerData.itemKeys.Count; i++)
		{
			ItemData itemData = DataManager.Instance.GetItemDataSet().GetItemData(playerData.itemKeys[i]);
			GameObject itemButton = Instantiate(Resources.Load(GlobalDataStructure.PATH_UIPREFAB_COMMON + "ItemButton")) as GameObject;
			itemButton.transform.SetParent(itemList, false);
			itemButton.GetComponent<CommandButtonUIEvent>().Init(itemData.name, itemData.description);
		}
	}
}
