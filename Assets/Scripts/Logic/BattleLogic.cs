using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class BattleLogic : MonoBehaviour {

	public static List<Enemy> enemys;
	public static List<Player> players;
	public static Command currentCommand;

	public Canvas battleCanvas;
	public Canvas mapCanvas;

	/*LIFE CYCLE */
	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (EventDefine.StartBattle, OnStartBattle);
		EventManager.Instance.RegisterEvent (EventDefine.PlayerReady, OnPlayerReady);
		EventManager.Instance.RegisterEvent (EventDefine.ClickCommand, OnClickCommand);
		EventManager.Instance.RegisterEvent (EventDefine.SelectCommand, OnSelectCommand);
		EventManager.Instance.RegisterEvent (EventDefine.ExecuteCommand, OnExecuteCommand);
		EventManager.Instance.RegisterEvent (EventDefine.BattleObjectDied, OnBattleObjectDied);
		EventManager.Instance.RegisterEvent (EventDefine.BattleObjectEscape, OnBattleObjectEscape);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (EventDefine.EnterBattle, OnStartBattle);
		EventManager.Instance.UnRegisterEvent (EventDefine.PlayerReady, OnPlayerReady);
		EventManager.Instance.UnRegisterEvent (EventDefine.ClickCommand, OnClickCommand);
		EventManager.Instance.UnRegisterEvent (EventDefine.SelectCommand, OnSelectCommand);
		EventManager.Instance.UnRegisterEvent (EventDefine.ExecuteCommand, OnExecuteCommand);
		EventManager.Instance.UnRegisterEvent (EventDefine.BattleObjectDied, OnBattleObjectDied);
		EventManager.Instance.UnRegisterEvent (EventDefine.BattleObjectEscape, OnBattleObjectEscape);
	}

	void Update()
	{
		if(GlobalManager.Instance.gameStatus == GameStatus.Battle)
			EventManager.Instance.PostEvent (EventDefine.UpdateTimeline);
	}

	/*UI CALLBACK*/
	public void EnterBattle()
	{
		mapCanvas.gameObject.SetActive (false);
		battleCanvas.gameObject.SetActive (true);

		enemys = new List<Enemy>();
		players = new List<Player>();

		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("Man","1");
		args.AddMessage("Girl","1");
		args.AddMessage("Enemy","10,10,10");
		EventManager.Instance.PostEvent (EventDefine.EnterBattle, args);
	}
	
	public void SelectBasicCommand(int commandID)
	{
		BasicCommand basicCommand = (BasicCommand)commandID;
		GetCurrentPlayer ().RefreshAvailableCommands (basicCommand);
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage("PlayerName",GetCurrentPlayer ().GetData().name);
		EventManager.Instance.PostEvent(EventDefine.ShowAvailableCommands, args);
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

	void OnStartBattle(MessageEventArgs args)
	{
		GlobalManager.Instance.gameStatus = GameStatus.Battle;
		ResumeEveryOne();
	}

	void OnPlayerReady(MessageEventArgs args)
	{
		PauseEveryOne();
	}

	void OnClickCommand(MessageEventArgs args)
	{
		int commandID = Convert.ToInt32(args.GetMessage("CommandID"));
		currentCommand = GetCurrentPlayer ().availableCommands.Find((Command cmd)=>{return cmd.commandID == commandID;});
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
			EventManager.Instance.PostEvent(EventDefine.SelectCommand);
			break;
		}
	}

	void OnSelectCommand(MessageEventArgs args)
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
			EventManager.Instance.PostEvent(EventDefine.BattleWin);

			StartCoroutine(FinishBattle());
		}
		else if(players.Count == 0)
		{
			EventManager.Instance.PostEvent(EventDefine.BattleLose);
			StartCoroutine(FinishBattle());
		}
	}

	void OnBattleObjectEscape(MessageEventArgs args)
	{
		EventManager.Instance.PostEvent(EventDefine.BattleLose);
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
		mapCanvas.gameObject.SetActive (true);
		battleCanvas.gameObject.SetActive (false);
		EventManager.Instance.PostEvent(EventDefine.FinishBattle);
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
