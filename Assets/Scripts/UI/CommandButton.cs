using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CommandButton : MonoBehaviour {
	
	string commandType;
	string commandName;
	string commandDescription;

	public void Init(string type, string name, string desc)
	{
		commandType = type;
		commandName = name;
		commandDescription = desc;

		GetComponentInChildren<Text>().text = commandName;
	}
	public void OnClickCommandButton()
	{
		Text descText = transform.parent.parent.FindChild("CommandDescription").GetComponent<Text>();
		descText.text = commandDescription;

		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage ("CommandType", commandType);
		args.AddMessage ("CommandName", commandName);
		args.AddMessage ("CommandDescription", commandDescription);
		EventManager.Instance.PostEvent (EventDefine.ClickCommand, args);
	}

}
