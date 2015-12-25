using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemButtonUIEvent : MonoBehaviour {
	
	public Text nameText;
	public Text descriptionText;
	
	public void Init(string name, string description)
	{
		nameText.text = name;
		descriptionText.text = description;
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
