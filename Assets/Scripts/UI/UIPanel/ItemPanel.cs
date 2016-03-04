using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemPanel : MonoBehaviour {

	private enum ViewMode{Consumable, Weapon, Accessory, KeyItem}

	public Transform itemList;

	private PlayerData playerData;
	private ViewMode viewMode;

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
		EventManager.Instance.PostEvent (UIEvent.OnMessageClear);
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
		EventManager.Instance.PostEvent (UIEvent.OnMessageSet, args);
	}

	void SetData()
	{
		playerData = DataManager.Instance.GetPlayerDataSet().GetPlayerData(DataManager.Instance.GetConfigData().currentLeaderID);
		SetViewMode((int)ViewMode.Consumable);
	}

	public void SetViewMode(int mode)
	{
		viewMode = (ViewMode)mode;
		InitItemButtons();
		EventManager.Instance.PostEvent (UIEvent.OnMessageClear);
	}

	private void InitItemButtons()
	{
		for(int i = 0; i < itemList.childCount; i++)
		{
			GameObject.Destroy(itemList.GetChild(i).gameObject);
		}
		foreach(var itemSlot in playerData.GetItemDict())
		{
			if(IsItemFitViewMode(itemSlot.Key))
			{
				GameObject itemButton = Instantiate(Resources.Load(GlobalDataStructure.PATH_UIPREFAB_COMMON + "ItemButton")) as GameObject;
				itemButton.transform.SetParent(itemList, false);				
				itemButton.GetComponent<ItemButtonUIEvent>().Init(itemSlot.Key, itemSlot.Value);
			}
		}
	}

	private bool IsItemFitViewMode(int itemID)
	{
		ItemData itemData = DataManager.Instance.GetItemDataSet().GetItemData(itemID);
		ItemType itemType = itemData.type;

		switch(viewMode)
		{
		case ViewMode.Consumable: return itemType == ItemType.Consumable;
		case ViewMode.Weapon: return itemType == ItemType.Weapon || itemType == ItemType.Magic;
		case ViewMode.Accessory: return itemType == ItemType.Accessory;
		case ViewMode.KeyItem: return itemType == ItemType.KeyItem;
		default: return false;
		}
	}
}
