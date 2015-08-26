using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameMusicLogic : MonoBehaviour {
	
	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (EventDefine.EnterBattle, PlayBattleBGM);
		EventManager.Instance.RegisterEvent (EventDefine.BattleWin, PlayIntro);
		EventManager.Instance.RegisterEvent (EventDefine.BattleLose, PlayIntro);
	}

	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (EventDefine.EnterBattle, PlayBattleBGM);
		EventManager.Instance.UnRegisterEvent (EventDefine.BattleWin, PlayIntro);
		EventManager.Instance.UnRegisterEvent (EventDefine.BattleLose, PlayIntro);
	}

	void PlayBattleBGM(MessageEventArgs args)
	{
		AudioManager.Instance.PlayBGM ("Music/Battle0" + UnityEngine.Random.Range(1,4));
	}

	void PlayIntro(MessageEventArgs args)
	{
		AudioManager.Instance.PlayBGM ("Music/Intro");
	}
}
