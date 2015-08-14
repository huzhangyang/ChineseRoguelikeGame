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
		EventManager.Instance.PostEvent (EventDefine.SelectCommand);
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
			if(enemy.battleStatus == BattleObject.BattleStatus.Ready)
			{
				//TODO store this enemy's command
				enemy.battleStatus = BattleObject.BattleStatus.Action;
			}
		}
		foreach(Player player in players)
		{
			if(player.battleStatus == BattleObject.BattleStatus.Ready)
			{
				//TODO store this player's command
				player.battleStatus = BattleObject.BattleStatus.Action;
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

	void Update()
	{
		if(GlobalManager.Instance.gameStatus == GlobalManager.GameStatus.Battle)
			EventManager.Instance.PostEvent (EventDefine.UpdateTimeline);
	}

}
