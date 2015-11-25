using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CommandButtonUIEvent : MonoBehaviour {

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
		args.AddMessage ("CommandName", name);
		EventManager.Instance.PostEvent (BattleEvent.OnCommandClicked, args);
	}

}
