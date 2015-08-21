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
		GetCurrentPlayer ().RefreshAvailableCommands (basicCommand);
		EventManager.Instance.PostEvent (EventDefine.ShowAvailableCommands, null);
		foreach(Enemy enemy in enemys)
		{
			enemy.GetComponent<BattleObjectUIEvent>().allowClick = false;
		}
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
		int commandID = Convert.ToInt32(args.GetMessage("CommandID"));
		currentCommand = GetCurrentPlayer ().availableCommands.Find((Command cmd)=>{return cmd.commandID == commandID;});
		if(currentCommand.commandType == CommandType.UseSkill)
		{
			switch(DataManager.Instance.GetSkillDataSet().GetSkillData(currentCommand.skillID).targetType)
			{
			case TargetType.SingleEnemy:
			case TargetType.AllEnemies:
				foreach(Enemy enemy in enemys)
				{
					enemy.GetComponent<BattleObjectUIEvent>().allowClick = true;
				}
				break;
			}
		}
		//available for click
	}

	void OnSelectCommand(MessageEventArgs args)
	{
		ResumeEveryOne();
		GetCurrentPlayer().commandToExecute = currentCommand;
		GetCurrentPlayer().battleStatus = BattleStatus.Action;
	}

	void OnExecuteCommand(MessageEventArgs args)
	{
		StartCoroutine(WaitEveryOne(1));
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
