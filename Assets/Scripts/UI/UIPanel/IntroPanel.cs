using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class IntroPanel : MonoBehaviour {

	Text text;
	string[] content = new string[1];
	int currentPlaying = 0;

	void OnEnable()
	{
		AudioManager.Instance.PlayBGM ("Intro");

		text = GetComponentInChildren<Text>();
		content[0] = "天不兼复兮，地不周载；\n火炎炎而不灭兮，水泱泱而不息。\n是谓之：洪荒。";

		currentPlaying = 0;
		PlayNextText();
	}

	public void OnClick()
	{
		PlayNextText();
	}

	void PlayNextText()
	{
		text.DOKill();
		text.text = "";
		if(currentPlaying < content.Length)
		{
			text.DOText(content[currentPlaying], 5).SetEase(Ease.Linear).SetDelay(1);
			currentPlaying++;
		}
		else
		{
			MessageEventArgs args = new MessageEventArgs();
			args.AddMessage("WindowID", UIWindowID.LoadingWindow);
			EventManager.Instance.PostEvent(UIEvent.OpenUIWindow, args);

			this.gameObject.SetActive(false);
		}

	}

}
