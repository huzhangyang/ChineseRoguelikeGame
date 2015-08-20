using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleLogic : MonoBehaviour {

	public static List<Enemy> enemys;
	public static List<Player> players;

	public Canvas battleCanvas;
	public Canvas mapCanvas;

	private string currentCommandName;

	/*LIFE CYCLE */
	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (EventDefine.StartBattle, OnStartBattle);
		EventManager.Instance.RegisterEvent (EventDefine.PlayerReady, OnPlayerReady);
		EventManager.Instance.RegisterEvent (EventDefine.ClickCommand, OnClickCommand);
		EventManager.Instance.RegisterEvent (EventDefine.SelectCommand, OnSelectCommand);
		EventManager.Instance.RegisterEvent (EventDefine.ExecuteCommand, OnExecuteCommand);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (EventDefine.EnterBattle, OnStartBattle);
		EventManager.Instance.UnRegisterEvent (EventDefine.PlayerReady, OnPlayerReady);
		EventManager.Instance.UnRegisterEvent (EventDefine.ClickCommand, OnClickCommand);
		EventManager.Instance.UnRegisterEvent (EventDefine.SelectCommand, OnSelectCommand);
		EventManager.Instance.UnRegisterEvent (EventDefine.ExecuteCommand, OnExecuteCommand);
	}

	void Update()
	{
		if(GlobalManager.Instance.gameStatus == GlobalManager.GameStatus.Battle)
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
		List<Command> commands = GetCurrentPlayer ().GetAvailableCommands (basicCommand);
		MessageEventArgs args = new MessageEventArgs ();
		args.AddMessage ("CommandCount", commands.Count.ToString ());
		for(int i = 0 ; i < commands.Count; i++)
		{
			args.AddMessage ("Command" + i +"Type", commands[i].commandType);
			args.AddMessage ("Command" + i +"Name", commands[i].commandName);
			args.AddMessage ("Command" + i +"Description", commands[i].commandDescription);
		}
		EventManager.Instance.PostEvent (EventDefine.ShowAvailableCommands, args);
		currentCommandName = "";
	}

	/*EVENT CALLBACK*/

	void OnStartBattle(MessageEventArgs args)
	{
		GlobalManager.Instance.gameStatus = GlobalManager.GameStatus.Battle;
		ResumeEveryOne();
	}

	void OnPlayerReady(MessageEventArgs args)
	{
		PauseEveryOne();
	}

	void OnClickCommand(MessageEventArgs args)
	{
		string commandName = args.GetMessage("CommandName");
		if(currentCommandName == commandName)
		{
			EventManager.Instance.PostEvent (EventDefine.SelectCommand, args);
		}
		else
		{
			currentCommandName = commandName;
		}
	}

	void OnSelectCommand(MessageEventArgs args)
	{
		ResumeEveryOne();
		string commandType = args.GetMessage("CommandType");
		string commandName = args.GetMessage("CommandName");
		string commandDescription = args.GetMessage("CommandDescription");
		GetCurrentPlayer().commandToExecute = Command.Build(commandType, commandName, commandDescription);
		GetCurrentPlayer().battleStatus = BattleStatus.Action;
	}

	void OnExecuteCommand(MessageEventArgs args)
	{
		StartCoroutine(WaitEveryOne(1));
	}

	/*CUSTOM METHOD*/

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
