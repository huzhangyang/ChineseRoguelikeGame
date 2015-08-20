using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CommandButton : MonoBehaviour {
	
	int commandID;
	string commandName;
	string description;

	public void Init(int commandID, string name, string description)
	{
		this.commandID = commandID;
		this.commandName = name;
		this.description = description;

		GetComponentInChildren<Text>().text = commandName;
	}
	public void OnClickCommandButton()
	{
		Text descText = transform.parent.parent.FindChild("CommandDescription").GetComponent<Text>();
		descText.text = description;

		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage ("CommandID", commandID.ToString());
		EventManager.Instance.PostEvent (EventDefine.ClickCommand, args);
	}

}
