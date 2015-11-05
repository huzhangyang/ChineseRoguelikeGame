using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CommandButtonUIEvent : MonoBehaviour {

	public void Init(string name, string description)
	{
		GetComponentInChildren<Text>().text = name;
		GetComponent<Button>().onClick.AddListener(delegate()  
		{  
			OnClick(name, description);  
		});		  
	}

	void OnClick(string name, string description)
	{
		Text descText = transform.parent.parent.parent.FindChild("CommandDescription").GetComponent<Text>();
		descText.text = description;

		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage ("CommandName", name);
		EventManager.Instance.PostEvent (BattleEvent.OnCommandClicked, args);
	}

}
