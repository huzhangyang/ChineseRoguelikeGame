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
		EventManager.Instance.RegisterEvent(UIEvent.OnItemClicked, OnItemClicked);
	}

	void OnDisable()
	{
		EventManager.Instance.UnRegisterEvent(UIEvent.OnSwitchLeader, OnSwitchLeader);
		EventManager.Instance.RegisterEvent(UIEvent.OnItemClicked, OnItemClicked);
	}

	void OnSwitchLeader(MessageEventArgs args)
	{
		SetData();
	}

	void OnItemClicked(MessageEventArgs args)
	{
		int itemID = args.GetMessage<int>("ItemID");
		ItemData itemData = DataManager.Instance.GetItemDataSet().GetItemData(itemID);
		args.AddMessage ("Message", itemData.description);
		EventManager.Instance.PostEvent (UIEvent.OnMessageShow, args);
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
