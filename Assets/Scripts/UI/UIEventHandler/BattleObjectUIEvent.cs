﻿using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class BattleObjectUIEvent : MonoBehaviour {

	private bool allowClick;
	private Image avatarImage;
	private Image objectImage;
	private Slider HPBar;

	void Start () 
	{
		allowClick = false;
		objectImage = this.GetComponent<Image>();
		GetComponent<Button>().onClick.AddListener(delegate(){OnClick();});		
	}

	public void SetHPBar(int current, int max)
	{
		if(HPBar == null)
			HPBar = this.gameObject.GetComponentInChildren<Slider> ();
		if(HPBar.maxValue != max)
		{
			HPBar.maxValue = max;
			HPBar.value = current;
		}
		else
		{
			HPBar.DOValue(current, 1).SetEase(Ease.OutSine);
			this.transform.DOPunchScale(new Vector2(0.1f, 0.1f), 1);
		}

		if(current == 0)
		{
			avatarImage.DOFade(0,1).SetDelay(1).OnComplete(()=>Destroy(avatarImage.gameObject));
			objectImage.DOFade(0,1).SetDelay(1).OnComplete(()=>Destroy(this.gameObject));
			Destroy(HPBar,1);
		}

	}
	
	public void SetAvatar(GameObject avatar)
	{
		avatarImage = avatar.GetComponent<Image>();
		avatarImage.sprite = this.GetComponent<Image> ().sprite;
	}

	public void SetAvatarPositionX(float posX, bool smooth)
	{
		if(smooth)
			avatarImage.rectTransform.DOLocalMoveX(posX - 250, 1).SetEase(Ease.OutSine);
		else
			avatarImage.rectTransform.anchoredPosition = new Vector2(posX, 0);			
	}

	public void EnableClick()
	{
		objectImage.DOColor(new Color(0.5f,0.5f,0.5f), 1f).SetLoops(-1,LoopType.Yoyo);
		allowClick = true;
	}

	public void DisableClick()
	{
		objectImage.color = new Color(1,1,1);
		objectImage.DOKill();
		allowClick = false;
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
