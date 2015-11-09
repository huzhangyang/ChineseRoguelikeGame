using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class BattleLogic : MonoBehaviour {

	public static List<Enemy> enemys;
	public static List<Player> players;
	public static Command currentCommand;



	/*LIFE CYCLE */
	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (BattleEvent.OnBattleEnter, OnBattleEnter);
		EventManager.Instance.RegisterEvent (BattleEvent.OnBattleStart, OnBattleStart);
		EventManager.Instance.RegisterEvent (BattleEvent.OnPlayerReady, OnPlayerReady);
		EventManager.Instance.RegisterEvent (BattleEvent.OnCommandClicked, OnCommandClicked);
		EventManager.Instance.RegisterEvent (BattleEvent.OnCommandSelected, OnCommandSelected);
		EventManager.Instance.RegisterEvent (BattleEvent.OnCommandExecute, OnExecuteCommand);
		EventManager.Instance.RegisterEvent (BattleEvent.BattleObjectDied, OnBattleObjectDied);
		EventManager.Instance.RegisterEvent (BattleEvent.BattleObjectEscape, OnBattleObjectEscape);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnBattleEnter, OnBattleEnter);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnBattleEnter, OnBattleStart);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnPlayerReady, OnPlayerReady);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnCommandClicked, OnCommandClicked);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnCommandSelected, OnCommandSelected);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnCommandExecute, OnExecuteCommand);
		EventManager.Instance.UnRegisterEvent (BattleEvent.BattleObjectDied, OnBattleObjectDied);
		EventManager.Instance.UnRegisterEvent (BattleEvent.BattleObjectEscape, OnBattleObjectEscape);
	}

	void Update()
	{
		if(GlobalManager.Instance.gameStatus == GameStatus.Battle)
			EventManager.Instance.PostEvent (BattleEvent.OnTimelineUpdate);
	}
	
	public void SelectBasicCommand(int commandID)
	{
		BasicCommand basicCommand = (BasicCommand)commandID;
		GetCurrentPlayer().RefreshAvailableCommands(basicCommand);
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("PlayerName", GetCurrentPlayer().GetName());
		EventManager.Instance.PostEvent(BattleEvent.OnBasicCommandSelected, args);
		foreach(Enemy enemy in enemys)
		{
			enemy.GetComponent<BattleObjectUIEvent>().DisableClick();
		}
		foreach(Player player in players)
		{
			player.GetComponent<BattleObjectUIEvent>().DisableClick();
		}
	}

	/*EVENT CALLBACK*/

	void OnBattleEnter(MessageEventArgs args)
	{		
		enemys = new List<Enemy>();
		players = new List<Player>();
	}


	void OnBattleStart(MessageEventArgs args)
	{
		GlobalManager.Instance.gameStatus = GameStatus.Battle;
		ResumeEveryOne();
	}

	void OnPlayerReady(MessageEventArgs args)
	{
		PauseEveryOne();
	}

	void OnCommandClicked(MessageEventArgs args)
	{
		string commandName = args.GetMessage<string>("CommandName");
		currentCommand = GetCurrentPlayer ().availableCommands.Find((Command cmd)=>{return cmd.commandName == commandName;});
		switch(currentCommand.targetType)
		{
		case TargetType.SingleEnemy:
		case TargetType.AllEnemies:
			foreach(Enemy enemy in enemys)
			{
				enemy.GetComponent<BattleObjectUIEvent>().EnableClick();
			}
			break;
		case TargetType.SingleAlly:
		case TargetType.AllAllies:
			foreach(Player player in players)
			{
				player.GetComponent<BattleObjectUIEvent>().EnableClick();
			}
			break;
		case TargetType.Self:
			EventManager.Instance.PostEvent(BattleEvent.OnCommandSelected);
			break;
		}
	}

	void OnCommandSelected(MessageEventArgs args)
	{
		ResumeEveryOne();
		GetCurrentPlayer().commandToExecute = currentCommand;
		GetCurrentPlayer().battleStatus = BattleStatus.Action;
		foreach(Enemy enemy in enemys)
		{
			enemy.GetComponent<BattleObjectUIEvent>().DisableClick();
		}
		foreach(Player player in players)
		{
			player.GetComponent<BattleObjectUIEvent>().DisableClick();
		}
	}

	void OnExecuteCommand(MessageEventArgs args)
	{
		StartCoroutine(WaitEveryOne(1));
	}

	void OnBattleObjectDied(MessageEventArgs args)
	{
		if(enemys.Count == 0)
		{
			EventManager.Instance.PostEvent(BattleEvent.OnBattleWin);

			StartCoroutine(FinishBattle());
		}
		else if(players.Count == 0)
		{
			EventManager.Instance.PostEvent(BattleEvent.OnBattleLose);
			StartCoroutine(FinishBattle());
		}
	}

	void OnBattleObjectEscape(MessageEventArgs args)
	{
		EventManager.Instance.PostEvent(BattleEvent.OnBattleLose);
		StartCoroutine(FinishBattle());
	}

	/*CUSTOM METHOD*/

	public static Player GetCurrentPlayer()
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

	IEnumerator WaitEveryOne(float seconds)
	{
		PauseEveryOne();
		yield return new WaitForSeconds(seconds);
		ResumeEveryOne();
	}

	IEnumerator FinishBattle()
	{
		GlobalManager.Instance.gameStatus = GameStatus.Map;
		yield return new WaitForSeconds(5);
		EventManager.Instance.PostEvent(BattleEvent.OnBattleFinish);
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
