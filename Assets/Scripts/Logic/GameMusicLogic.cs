using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameMusicLogic : MonoBehaviour {
	
	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (BattleEvent.OnBattleEnter, PlayBattleBGM);
		EventManager.Instance.RegisterEvent (BattleEvent.OnBattleFinish, PlayIntro);
		EventManager.Instance.RegisterEvent (BattleEvent.OnBattleWin, PlayWin);
		EventManager.Instance.RegisterEvent (BattleEvent.OnBattleLose, PlayLose);
	}

	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnBattleEnter, PlayBattleBGM);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnBattleFinish, PlayIntro);
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

	void PlayIntro(MessageEventArgs args)
	{
		AudioManager.Instance.PlayBGM ("Intro");
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
