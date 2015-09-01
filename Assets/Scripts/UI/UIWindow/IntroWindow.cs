using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class IntroWindow : MonoBehaviour {

	public Image bookLight;
	public GameObject intro;
	public GameObject continueButton;

	void Start () 
	{
		DataManager.Instance.LoadConfig();
		AudioManager.Instance.PlayBGM ("Theme");
		DOTween.ToAlpha(()=>bookLight.color, (x)=> bookLight.color = x , 0 , 2).SetLoops(-1, LoopType.Yoyo);
		if (!SaveManager.Instance.IsSaveExist())
			continueButton.SetActive(false);
	}

	public void OnButtonNew()
	{
		GlobalManager.Instance.gameStatus = GameStatus.StartNewGame;
		intro.SetActive(true);
	}
	
	public void OnButtonContinue()
	{
		Application.LoadLevel("LoadingScene");
	}
	
	public void OnButtonSetting()
	{
		
	}
	
	public void OnButtonExit()
	{
		Application.Quit();
	}

}
