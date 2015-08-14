using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleLogic : MonoBehaviour {

	public static List<Enemy> enemys;
	public static List<Player> players;

	public Canvas battleCanvas;
	public Canvas mapCanvas;

	public enum Level1Command {Attack,Defence,Item,Strategy}
	public enum Level2AttackCommand{}
	public enum Level2DefenceCommand{}
	public enum Level2ItemCommand{}
	public enum Level2StrategyCommand{}

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (EventDefine.StartBattle, OnStartBattle);
		EventManager.Instance.RegisterEvent (EventDefine.PlayerReady, OnPlayerReady);
		EventManager.Instance.RegisterEvent (EventDefine.DecideCommand, OnDecideCommand);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (EventDefine.EnterBattle, OnStartBattle);
		EventManager.Instance.UnRegisterEvent (EventDefine.PlayerReady, OnPlayerReady);
		EventManager.Instance.UnRegisterEvent (EventDefine.DecideCommand, OnDecideCommand);
	}

	public void EnterBattle()
	{
		mapCanvas.gameObject.SetActive (false);
		battleCanvas.gameObject.SetActive (true);

		enemys = new List<Enemy>();
		players = new List<Player>();

		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("man","1");
		args.AddMessage("girl","1");
		args.AddMessage("enemy","10,10,10");
		EventManager.Instance.PostEvent (EventDefine.EnterBattle, args);
	}

	public void SelectLevel1Command(int commandID)
	{
		Level1Command command = (Level1Command)commandID;
		switch(command)
		{
		case Level1Command.Attack:
			break;
		case Level1Command.Defence:
			break;
		case Level1Command.Item:
			break;
		case Level1Command.Strategy:
			break;
		}
		Debug.Log("Command" + commandID);
		EventManager.Instance.PostEvent (EventDefine.DecideCommand);
	}

	void OnStartBattle(MessageEventArgs args)
	{
		GlobalManager.Instance.gameStatus = GlobalManager.GameStatus.Battle;
	}

	void OnPlayerReady(MessageEventArgs args)
	{
		foreach(Enemy enemy in enemys)
		{
			enemy.isPaused = true;
		}
		foreach(Player player in players)
		{
			player.isPaused = true;
		}
	}

	void OnDecideCommand(MessageEventArgs args)
	{
		Debug.Log("OnDecideCommand_logic");
		foreach(Enemy enemy in enemys)
		{
			enemy.isPaused = false;
			if(enemy.battleStatus == BattleObject.BattleStatus.Ready)
			{
				//TODO store this enemy's command
				enemy.battleStatus = BattleObject.BattleStatus.Action;
			}
		}
		foreach(Player player in players)
		{
			player.isPaused = false;
			if(player.battleStatus == BattleObject.BattleStatus.Ready)
			{
				//TODO store this player's command
				player.battleStatus = BattleObject.BattleStatus.Action;
			}
		}
	}

	void Update()
	{
		if(GlobalManager.Instance.gameStatus == GlobalManager.GameStatus.Battle)
			EventManager.Instance.PostEvent (EventDefine.UpdateTimeline);
	}

}
