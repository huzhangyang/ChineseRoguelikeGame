using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class BattleLogic : MonoBehaviour {

	public Text message;
	public Image bossPic;
	public GameObject buttons;

	public void Fight()
	{
		message.text = "BOSS被你打败了！";
		bossPic.sprite = Resources.Load("boss1", typeof(Sprite)) as Sprite;
		buttons.SetActive (false);
	}

	public void Run()
	{
		message.text = "逃走了...";
		bossPic.gameObject.SetActive (false);
		buttons.SetActive (false);
	}

	public void Stay()
	{
		message.text = "被BOSS杀死了！";
		buttons.SetActive (false);
		bossPic.transform.DOShakeScale(1f, new Vector3(0.3f, -0.3f, 0f), 4, 0f).SetLoops(-1, LoopType.Yoyo);
	}
}
