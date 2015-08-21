using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CommandButtonUIEvent : MonoBehaviour {

	public void Init(int commandID, string name, string description)
	{
		GetComponentInChildren<Text>().text = name;
		GetComponent<Button>().onClick.AddListener(delegate()  
		{  
			OnClick(commandID, description);  
		});		  
	}

	void OnClick(int commandID, string description)
	{
		Text descText = transform.parent.parent.FindChild("CommandDescription").GetComponent<Text>();
		descText.text = description;

		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage ("CommandID", commandID.ToString());
		EventManager.Instance.PostEvent (EventDefine.ClickCommand, args);
	}

}
