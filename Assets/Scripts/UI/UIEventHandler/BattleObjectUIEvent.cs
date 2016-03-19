using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class BattleObjectUIEvent : MonoBehaviour {

	private Image avatarImage;
	private Slider HPBar;
	private Text HPText;
	private bool allowClick;

	public Image objectImage;
	public Image shadowImage;
	public Transform buffTransform;

	public void Init(int playerID) 
	{
		allowClick = false;
		objectImage.sprite = Resources.Load(GlobalDataStructure.PATH_UIIMAGE_BATTLE + "Avatar" + playerID.ToString("00"), typeof(Sprite)) as Sprite;
		shadowImage.rectTransform.sizeDelta = new Vector2 (objectImage.sprite.rect.width, objectImage.sprite.rect.height);
		InitAvatar();
		GetComponent<Button>().onClick.AddListener(delegate(){OnClick();});		
	}

	public void DestoryUI()
	{
		avatarImage.DOFade(0,1).SetDelay(1).OnComplete(()=>Destroy(avatarImage.gameObject));
		objectImage.DOFade(0,1).SetDelay(1).OnComplete(()=>Destroy(this.gameObject));
		Destroy(HPBar,1);
	}

	public void InitHPBar(int max, BattleType type)
	{
		GameObject bar = Instantiate(Resources.Load(GlobalDataStructure.PATH_UIPREFAB_BATTLE + "HPBar")) as GameObject;
		bar.transform.SetParent(this.transform, false);
		HPBar = bar.GetComponent<Slider>();
		HPBar.maxValue = max;
		HPBar.value = max;
		HPText = bar.GetComponentInChildren<Text>();
		HPText.text = HPBar.value + "/" + HPBar.maxValue;

		Color hpColor = Color.green;
		if(type == BattleType.Physical) hpColor = Color.red;
		else if(type == BattleType.Magical) hpColor = Color.blue;
		HPBar.transform.FindChild("Fill Area").GetComponentInChildren<Image>().color = hpColor;
	}

	public void InitEnemyHPBar(int max, BattleType type)
	{
		GameObject bar = Instantiate(Resources.Load(GlobalDataStructure.PATH_UIPREFAB_BATTLE + "EnemyHPBar")) as GameObject;
		bar.transform.SetParent(this.transform, false);
		bar.transform.localPosition = new Vector3(0, shadowImage.rectTransform.sizeDelta.y / 2, 0); 
		((RectTransform)bar.transform).sizeDelta = new Vector2 (shadowImage.rectTransform.sizeDelta.x / 2, 32);
		HPBar = bar.GetComponent<Slider>();
		HPBar.maxValue = max;
		HPBar.value = max;
		HPText = bar.GetComponentInChildren<Text>();
		HPText.gameObject.SetActive(false);
		HPText.text = HPBar.value + "/" + HPBar.maxValue;

		Color hpColor = Color.green;
		if(type == BattleType.Physical) hpColor = Color.red;
		else if(type == BattleType.Magical) hpColor = Color.blue;
		HPBar.transform.FindChild("Fill Area").GetComponentInChildren<Image>().color = hpColor;
	}

	public void SetHPBar(int current)
	{
		HPBar.DOValue(current, 1).SetEase(Ease.OutSine);
		HPText.text = current + "/" + HPBar.maxValue;
	}

	public void SetHPBar(int current, int max)
	{
		HPBar.maxValue = max;
		SetHPBar(current);
	}
	
	public void InitAvatar()
	{
		GameObject avatar = Instantiate(Resources.Load(GlobalDataStructure.PATH_UIPREFAB_BATTLE + "Avatar")) as GameObject;
		avatar.transform.SetParent(GameObject.Find("TimeLine").transform, false);
		avatarImage = avatar.GetComponent<Image>();
		avatarImage.sprite = objectImage.sprite;
		avatarImage.rectTransform.anchoredPosition = new Vector2 (0, 35);
		avatarImage.rectTransform.sizeDelta = new Vector2 (50 * objectImage.sprite.rect.width / objectImage.sprite.rect.height, 50);
	}

	public void SetAvatarPositionX(float posX)
	{
		if(BattleManager.Instance.IsBattlePaused())
			avatarImage.rectTransform.DOLocalMoveX(posX - 250, 1).SetEase(Ease.OutSine);
		else
			avatarImage.rectTransform.anchoredPosition = new Vector2(posX, 35);			
	}

	/*---------- Effect ----------*/

	public void OnDamage()
	{
		if(!DOTween.IsTweening(this.transform))
			this.transform.DOPunchScale(new Vector2(0.1f, 0.1f), 1);
	}

	public void OnHeal()
	{
		this.transform.DOPunchScale(new Vector2(0.1f, 0.1f), 1);
	}

	public void BeginReady()
	{
		avatarImage.transform.DOScale(new Vector3(1.2f, 1.2f), 1).SetLoops(-1,LoopType.Yoyo);
		avatarImage.transform.SetAsLastSibling ();
		shadowImage.sprite = Resources.Load(GlobalDataStructure.PATH_UIIMAGE_COMMON + "AvatarBack_Light", typeof(Sprite)) as Sprite;
	}

	public void EndReady()
	{
		avatarImage.transform.DOKill ();
		avatarImage.transform.localScale = new Vector3 (1, 1, 1);
		shadowImage.sprite = Resources.Load(GlobalDataStructure.PATH_UIIMAGE_COMMON + "AvatarBack_Dark", typeof(Sprite)) as Sprite;
	}

	public void BeginExecute()
	{
		shadowImage.sprite = Resources.Load(GlobalDataStructure.PATH_UIIMAGE_COMMON + "AvatarBack_Light", typeof(Sprite)) as Sprite;
	}

	public void EndExecute()
	{
		shadowImage.sprite = Resources.Load(GlobalDataStructure.PATH_UIIMAGE_COMMON + "AvatarBack_Dark", typeof(Sprite)) as Sprite;
	}

	public void EnableClick()
	{
		if(DOTween.IsTweening(objectImage))
		{
			DOTween.Restart(objectImage);
		}
		else
		{
			objectImage.DOColor(new Color(0.5f,0.5f,0.5f), 1f).SetLoops(-1,LoopType.Yoyo);
		}
		allowClick = true;
	}

	public void DisableClick()
	{
		objectImage.color = new Color(1,1,1);
		objectImage.DOKill();
		allowClick = false;
	}

	/*---------- Callback ----------*/

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
