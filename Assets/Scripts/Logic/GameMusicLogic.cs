using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameMusicLogic : MonoBehaviour {
	
	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (EventDefine.EnterBattle, PlayBattleBGM);
	}

	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (EventDefine.EnterBattle, PlayBattleBGM);
	}

	void PlayBattleBGM(MessageEventArgs args)
	{
		AudioManager.Instance.PlayBGM ("Music/Battle0" + UnityEngine.Random.Range(1,4));
	}
}
