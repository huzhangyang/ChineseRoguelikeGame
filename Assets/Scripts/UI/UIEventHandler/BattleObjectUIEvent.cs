using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleObjectUIEvent : MonoBehaviour {

	public bool allowClick;
	private Image avatarImage;
	private Slider HPBar;

	void Start () 
	{
		allowClick = false;
		GetComponent<Button>().onClick.AddListener(delegate(){OnClick();});		
	}

	public void SetHPBar(int current, int max)
	{
		if(HPBar == null)
			HPBar = this.gameObject.GetComponentInChildren<Slider> ();
		HPBar.maxValue = max;
		HPBar.value = current;
	}
	
	public void SetAvatar(GameObject avatar)
	{
		avatarImage = avatar.GetComponent<Image>();
		avatarImage.sprite = this.GetComponent<Image> ().sprite;
	}

	public void SetAvatarPositionX(float posX)
	{
		avatarImage.rectTransform.anchoredPosition = new Vector2(posX, 0);
	}

	public void DestroyAvatar()
	{
		Destroy(avatarImage.gameObject);
	}

	void OnClick()
	{
		if(allowClick)
		{
			BattleLogic.currentCommand.target = this.GetComponent<BattleObject>();
			EventManager.Instance.PostEvent(EventDefine.SelectCommand);
			allowClick = false;
		}
	}

}
