using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class IntroWindow : MonoBehaviour {

	public Image bookLight;

	void Start () 
	{
		AudioManager.Instance.PlayBGM ("Theme");
		DOTween.ToAlpha(()=>bookLight.color, (x)=> bookLight.color = x , 0 , 2).SetLoops(-1, LoopType.Yoyo);
	}

	public void OnButtonNew()
	{
		Application.LoadLevel("LoadingScene");
	}
	
	public void OnButtonContinue()
	{
		
	}
	
	public void OnButtonSetting()
	{
		
	}
	
	public void OnButtonExit()
	{
		Application.Quit();
	}

}
