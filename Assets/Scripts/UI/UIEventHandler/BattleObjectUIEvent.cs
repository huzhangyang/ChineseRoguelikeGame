using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class BattleObjectUIEvent : MonoBehaviour {

	private bool allowClick;
	private Image avatarImage;
	private Image objectImage;
	private Slider HPBar;
	private Text HPText;

	public void Init(int playerID) 
	{
		allowClick = false;
		objectImage = this.GetComponent<Image>();
		if(playerID < 10)
			objectImage.sprite = Resources.Load("UI/Battle/Avatar0" + playerID, typeof(Sprite)) as Sprite;
		else
		{
			objectImage.sprite = Resources.Load("UI/Battle/Avatar" + playerID, typeof(Sprite)) as Sprite;
			objectImage.rectTransform.sizeDelta = new Vector2 (objectImage.sprite.rect.width, objectImage.sprite.rect.height);
		}
		InitAvatar();
		GetComponent<Button>().onClick.AddListener(delegate(){OnClick();});		
	}

	public void InitHPBar(int max, BattleType type)
	{
		GameObject bar = Instantiate(Resources.Load(GlobalDataStructure.PATH_BATTLE + "HPBar")) as GameObject;
		bar.transform.SetParent(this.transform, false);
		HPBar = bar.GetComponent<Slider>();
		HPBar.maxValue = max;
		HPBar.value = max;
		HPText = bar.GetComponentInChildren<Text>();
		HPText.text = HPBar.value + "/" + HPBar.maxValue;

		/*Color hpColor = Color.green;
		if(type == BattleType.Physical) hpColor = Color.red;
		else if(type == BattleType.Magical) hpColor = Color.blue;
		HPBar.transform.FindChild("Fill Area").GetComponentInChildren<Image>().color = hpColor;*/
	}

	public void InitEnemyHPBar(int max, BattleType type)
	{
		GameObject bar = Instantiate(Resources.Load(GlobalDataStructure.PATH_BATTLE + "EnemyHPBar")) as GameObject;
		bar.transform.SetParent(this.transform, false);
		bar.transform.localPosition = new Vector3(0, objectImage.rectTransform.sizeDelta.y / 2, 0); 
		((RectTransform)bar.transform).sizeDelta = new Vector2 (objectImage.rectTransform.sizeDelta.x / 2, 32);
		HPBar = bar.GetComponent<Slider>();
		HPBar.maxValue = max;
		HPBar.value = max;
		HPText = bar.GetComponentInChildren<Text>();
		HPText.gameObject.SetActive(false);
		HPText.text = HPBar.value + "/" + HPBar.maxValue;

		/*Color hpColor = Color.green;
		if(type == BattleType.Physical) hpColor = Color.red;
		else if(type == BattleType.Magical) hpColor = Color.blue;
		HPBar.transform.FindChild("Fill Area").GetComponentInChildren<Image>().color = hpColor;*/
	}

	public void SetHPBar(int current)
	{
		HPBar.DOValue(current, 1).SetEase(Ease.OutSine);
		HPText.text = current + "/" + HPBar.maxValue;
		this.transform.DOPunchScale(new Vector2(0.1f, 0.1f), 1);
	}

	public void DestoryUI()
	{
		avatarImage.DOFade(0,1).SetDelay(1).OnComplete(()=>Destroy(avatarImage.gameObject));
		objectImage.DOFade(0,1).SetDelay(1).OnComplete(()=>Destroy(this.gameObject));
		Destroy(HPBar,1);
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
