using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleLogic : MonoBehaviour {

	public static List<Enemy> enemys;
	public static List<Player> players;

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (EventDefine.StartBattle, StartBattle);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (EventDefine.EnterBattle, StartBattle);
	}

	public void EnterBattle()
	{
		enemys = new List<Enemy>();
		players = new List<Player>();

		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("man","1");
		args.AddMessage("girl","1");
		args.AddMessage("enemy","10,10,10");
		EventManager.Instance.PostEvent (EventDefine.EnterBattle, args);
	}

	void StartBattle(MessageEventArgs args)
	{
		GlobalManager.Instance.gameStatus = GlobalManager.GameStatus.Battle;
		foreach(Player player in players)
		{
			player.battleStatus = BattleObject.BattleStatus.Prepare;
		}
	}

	void Update()
	{
		if(GlobalManager.Instance.gameStatus == GlobalManager.GameStatus.Battle)
			EventManager.Instance.PostEvent (EventDefine.UpdateTimeline, new MessageEventArgs ());
	}

}
