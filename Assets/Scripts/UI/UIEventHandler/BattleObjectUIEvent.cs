using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleObjectUIEvent : MonoBehaviour {

	public bool allowClick;

	void Start () 
	{
		allowClick = false;
		GetComponent<Button>().onClick.AddListener(delegate()  
		{  
			OnClick();  
		});		
	}

	void OnClick()
	{
		if(allowClick)
		{
			BattleLogic.currentCommand.target = this.GetComponent<BattleObject>();
			EventManager.Instance.PostEvent(EventDefine.SelectCommand);
		}
	}

}
