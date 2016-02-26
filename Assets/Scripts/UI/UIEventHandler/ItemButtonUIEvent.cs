using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemButtonUIEvent : MonoBehaviour {
	
	public Text nameText;
	public Text descriptionText;
	public Text numText;
	
	public void Init(int itemID, int num)
	{
		ItemData itemData = DataManager.Instance.GetItemDataSet().GetItemData(itemID);

		nameText.text = itemData.name;
		descriptionText.text = itemData.description;
		numText.text = "*" + num.ToString();
		GetComponent<Button>().onClick.AddListener(delegate()  
		                                           {  
			OnClick(name);  
		});		  
	}
	
	void OnClick(string name)
	{
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage ("ItemName", name);
		EventManager.Instance.PostEvent (UIEvent.OnItemClicked, args);
	}
	
}
