using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemButtonUIEvent : MonoBehaviour {
	
	public Text nameText;
	public Text descriptionText;
	public Text numText;

	ItemData itemData;

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent(UIEvent.OnItemClicked, OnItemClicked);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent(UIEvent.OnItemClicked, OnItemClicked);
	}
	
	public void Init(int itemID, int num)
	{
		itemData = DataManager.Instance.GetItemDataSet().GetItemData(itemID);

		nameText.text = itemData.name;
		descriptionText.text = itemData.shortDesc;
		numText.text = "*" + num.ToString();
		GetComponent<Button>().onClick.AddListener(delegate()  
		                                           {  
			OnClick(name);  
		});		  
	}
	
	void OnClick(string name)
	{
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage ("ItemID", itemData.id);
		EventManager.Instance.PostEvent (UIEvent.OnItemClicked, args);
	}

	void OnItemClicked(MessageEventArgs args)
	{
		if(args.GetMessage<int>("ItemID") == itemData.id)
		{
			this.GetComponent<Image>().sprite = Resources.Load(GlobalDataStructure.PATH_UIIMAGE_COMMON + "Box_Light", typeof(Sprite)) as Sprite;
		}
		else
		{
			this.GetComponent<Image>().sprite = Resources.Load(GlobalDataStructure.PATH_UIIMAGE_COMMON + "Box_Dark", typeof(Sprite)) as Sprite;
		}
	}
	
}
