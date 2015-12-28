using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameMusicLogic : MonoBehaviour {
	
	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (UIEvent.OpenUIWindow, PlayBGM);
		EventManager.Instance.RegisterEvent (BattleEvent.OnBattleEnter, PlayBattleBGM);
		EventManager.Instance.RegisterEvent (BattleEvent.OnBattleWin, PlayWin);
		EventManager.Instance.RegisterEvent (BattleEvent.OnBattleLose, PlayLose);
	}

	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (UIEvent.OpenUIWindow, PlayBGM);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnBattleEnter, PlayBattleBGM);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnBattleWin, PlayWin);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnBattleLose, PlayLose);
	}

	void PlayBattleBGM(MessageEventArgs args)
	{
		int battleType = args.GetMessage<int>("BattleType");
		if(battleType == 0)
			AudioManager.Instance.PlayBGM ("Battle0" + UnityEngine.Random.Range(1,4));
		else if(battleType == 1)
			AudioManager.Instance.PlayBGM ("Boss0" + UnityEngine.Random.Range(1,3));
		else if(battleType == 2)
			AudioManager.Instance.PlayBGM ("FinalBoss");
	}

	void PlayBGM(MessageEventArgs args)
	{
		UIWindowID windowID = args.GetMessage<UIWindowID>("WindowID");
		if(windowID == UIWindowID.IntroWindow)
		{
			AudioManager.Instance.PlayBGM ("Theme");
		}
		else if(windowID == UIWindowID.MapWindow)
		{
			AudioManager.Instance.PlayBGM ("Map");
		}
	}

	void PlayWin(MessageEventArgs args)
	{
		AudioManager.Instance.PlayBGM ("Win0" + UnityEngine.Random.Range(1,3));
	}

	void PlayLose(MessageEventArgs args)
	{
		AudioManager.Instance.PlayBGM ("Lose");
	}
}
