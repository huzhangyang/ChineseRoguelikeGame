using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleLogic : MonoBehaviour {

	public static List<Enemy> enemys;
	public static List<Player> players;

	public Canvas battleCanvas;
	public Canvas mapCanvas;

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (EventDefine.StartBattle, OnStartBattle);
		EventManager.Instance.RegisterEvent (EventDefine.PlayerReady, OnPlayerReady);
		EventManager.Instance.RegisterEvent (EventDefine.SelectCommand, OnSelectCommand);
		EventManager.Instance.RegisterEvent (EventDefine.ExecuteCommand, OnExecuteCommand);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (EventDefine.EnterBattle, OnStartBattle);
		EventManager.Instance.UnRegisterEvent (EventDefine.PlayerReady, OnPlayerReady);
		EventManager.Instance.UnRegisterEvent (EventDefine.SelectCommand, OnSelectCommand);
		EventManager.Instance.UnRegisterEvent (EventDefine.ExecuteCommand, OnExecuteCommand);
	}

	void Update()
	{
		if(GlobalManager.Instance.gameStatus == GlobalManager.GameStatus.Battle)
			EventManager.Instance.PostEvent (EventDefine.UpdateTimeline);
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

	public void SelectBasicCommand(int commandID)
	{
		BasicCommand basicCommand = (BasicCommand)commandID;
		List<Command> commands = GetCurrentPlayer ().GetAvailableCommands (basicCommand);
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage ("commandCount", commands.Count.ToString ());
		for(int i = 0 ; i < commands.Count; i++)
		{
			args.AddMessage ("command" + i +"Type", commands[i].commandType);
			args.AddMessage ("command" + i +"Name", commands[i].commandName);
			args.AddMessage ("command" + i +"Description", commands[i].commandDescription);
		}
		EventManager.Instance.PostEvent (EventDefine.ShowAvailableCommands, args);
		//EventManager.Instance.PostEvent (EventDefine.SelectCommand);
	}

	public Player GetCurrentPlayer()
	{
		foreach(Player player in players)
		{
			if(player.battleStatus == BattleStatus.Ready)
			{
				return player;
			}
		}
		Debug.LogError("There is no player ready.");
		return null;
	}

	void OnStartBattle(MessageEventArgs args)
	{
		GlobalManager.Instance.gameStatus = GlobalManager.GameStatus.Battle;
		ResumeEveryOne();
	}

	void OnPlayerReady(MessageEventArgs args)
	{
		PauseEveryOne();
	}

	void OnSelectCommand(MessageEventArgs args)
	{
		ResumeEveryOne();
		foreach(Enemy enemy in enemys)
		{
			if(enemy.battleStatus == BattleStatus.Ready)
			{
				//TODO store this enemy's command
				enemy.battleStatus = BattleStatus.Action;
			}
		}
		foreach(Player player in players)
		{
			if(player.battleStatus == BattleStatus.Ready)
			{
				//TODO store this player's command
				player.battleStatus = BattleStatus.Action;
			}
		}
	}

	void OnExecuteCommand(MessageEventArgs args)
	{
		StartCoroutine(WaitEveryOne(1));
	}

	IEnumerator WaitEveryOne(float seconds)
	{
		PauseEveryOne();
		yield return new WaitForSeconds(seconds);
		ResumeEveryOne();
	}

	void PauseEveryOne()
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

	void ResumeEveryOne()
	{
		foreach(Enemy enemy in enemys)
		{
			enemy.isPaused = false;
		}
		foreach(Player player in players)
		{
			player.isPaused = false;
		}
	}



}
