using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CommandButtonUIEvent : MonoBehaviour {

	public Text nameText;
	public Text descriptionText;

	public void Init(string name, string description, bool availAble)
	{
		nameText.text = name;
		descriptionText.text = description;

		if(availAble)
		{
			GetComponent<Button>().onClick.AddListener(delegate(){OnClick(name);});	
			nameText.color = Color.black;
			descriptionText.color = Color.black;
		}
		else
		{
			nameText.color = Color.gray;
			descriptionText.color = Color.gray;
		}  
	}

	void OnClick(string name)
	{
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage ("CommandName", name);
		EventManager.Instance.PostEvent (BattleEvent.OnCommandClicked, args);
	}

}
