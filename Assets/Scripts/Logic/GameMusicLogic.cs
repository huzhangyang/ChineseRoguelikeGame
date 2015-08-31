using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameMusicLogic : MonoBehaviour {
	
	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (EventDefine.EnterBattle, PlayBattleBGM);
		EventManager.Instance.RegisterEvent (EventDefine.FinishBattle, PlayIntro);
		EventManager.Instance.RegisterEvent (EventDefine.BattleWin, PlayWin);
		EventManager.Instance.RegisterEvent (EventDefine.BattleLose, PlayLose);
	}

	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (EventDefine.EnterBattle, PlayBattleBGM);
		EventManager.Instance.UnRegisterEvent (EventDefine.FinishBattle, PlayIntro);
		EventManager.Instance.UnRegisterEvent (EventDefine.BattleWin, PlayWin);
		EventManager.Instance.UnRegisterEvent (EventDefine.BattleLose, PlayLose);
	}

	void PlayBattleBGM(MessageEventArgs args)
	{
		int battleType = Convert.ToInt32(args.GetMessage("BattleType"));
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
