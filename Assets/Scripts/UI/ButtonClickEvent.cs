﻿using UnityEngine;
using System.Collections;

public class ButtonClickEvent: MonoBehaviour {

	public void IntroScene_New()
	{
		Application.LoadLevel("LoadingScene");
	}

	public void IntroScene_Continue()
	{

	}

	public void IntroScene_Setting()
	{

	}

	public void IntroScene_Exit()
	{
		Application.Quit();
	}
}
