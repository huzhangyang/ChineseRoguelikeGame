using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class BattleObjectUIEvent : MonoBehaviour {

	private bool allowClick;
	private Image avatarImage;
	private Image objectImage;
	private Slider HPBar;

	public void Init(int playerID) 
	{
		allowClick = false;
		objectImage = this.GetComponent<Image>();
		if(playerID < 10)
			objectImage.sprite = Resources.Load("UI/Battle/Avatar0" + playerID, typeof(Sprite)) as Sprite;
		else
			objectImage.sprite = Resources.Load("UI/Battle/Avatar" + playerID, typeof(Sprite)) as Sprite;
		InitAvatar();
		GetComponent<Button>().onClick.AddListener(delegate(){OnClick();});		
	}

	public void InitHPBar(int current, int max, BattleType type)
	{
		GameObject bar = Instantiate(Resources.Load(GlobalDataStructure.PATH_BATTLE + "HPBar")) as GameObject;
		bar.transform.SetParent(this.transform, false);
		bar.transform.localPosition = new Vector3(0, objectImage.rectTransform.sizeDelta.y / 2, 0); 
		HPBar = bar.GetComponent<Slider>();
		HPBar.maxValue = max;
		HPBar.value = current;

		Color hpColor = Color.green;
		if(type == BattleType.Physical) hpColor = Color.red;
		else if(type == BattleType.Magical) hpColor = Color.blue;
		HPBar.transform.FindChild("Fill Area").GetComponentInChildren<Image>().color = hpColor;
	}

	public void InitEnemyHPBar(int current, int max, BattleType type)
	{
		GameObject bar = Instantiate(Resources.Load(GlobalDataStructure.PATH_BATTLE + "HPBar")) as GameObject;
		bar.transform.SetParent(this.transform, false);
		bar.transform.localPosition = new Vector3(0, objectImage.rectTransform.sizeDelta.y / 2, 0); 
		bar.transform.localScale = new Vector3(objectImage.rectTransform.sizeDelta.x / 600, 1);
		HPBar = bar.GetComponent<Slider>();
		HPBar.maxValue = max;
		HPBar.value = current;

		Color hpColor = Color.green;
		if(type == BattleType.Physical) hpColor = Color.red;
		else if(type == BattleType.Magical) hpColor = Color.blue;
		HPBar.transform.FindChild("Fill Area").GetComponentInChildren<Image>().color = hpColor;
	}

	public void SetHPBar(int current)
	{
		HPBar.DOValue(current, 1).SetEase(Ease.OutSine);
		this.transform.DOPunchScale(new Vector2(0.1f, 0.1f), 1);

		if(current == 0)
		{
			avatarImage.DOFade(0,1).SetDelay(1).OnComplete(()=>Destroy(avatarImage.gameObject));
			objectImage.DOFade(0,1).SetDelay(1).OnComplete(()=>Destroy(this.gameObject));
			Destroy(HPBar,1);
		}
	}
	
	public void InitAvatar()
	{
		GameObject avatar = Instantiate(Resources.Load(GlobalDataStructure.PATH_BATTLE + "Avatar")) as GameObject;
		avatar.transform.SetParent(GameObject.Find("TimeLine").transform, false);
		avatarImage = avatar.GetComponent<Image>();
		avatarImage.sprite = objectImage.sprite;
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
			MessageEventArgs args = new MessageEventArgs();
			args.AddMessage("Target", this.GetComponent<BattleObject>());
			EventManager.Instance.PostEvent(BattleEvent.OnCommandSelected, args);
		}
	}

}
