using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class BattleManager : MonoBehaviour {

/*
战斗管理器
*/

	private static BattleManager s_Instance;
	public BattleManager() { s_Instance = this; }
	public static BattleManager Instance { get { return s_Instance; } }

	private List<Enemy> enemys = new List<Enemy>();
	private List<Player> players = new List<Player>();
	private Player currentPlayer;//当前在选择指令的角色
	private Command currentCommand;//当前角色选择的指令
	private Queue commandQueue = new Queue();//指令序列
	private Queue readyQueue = new Queue();//Ready序列
	private bool isPaused;//战斗是否暂停

	/*LIFE CYCLE */
	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (BattleEvent.OnBattleEnter, OnBattleEnter);
		EventManager.Instance.RegisterEvent (BattleEvent.OnEnemySpawn, OnEnemySpawn);
		EventManager.Instance.RegisterEvent (BattleEvent.OnPlayerSpawn, OnPlayerSpawn);
		EventManager.Instance.RegisterEvent (BattleEvent.OnBattleStart, OnBattleStart);
		EventManager.Instance.RegisterEvent (BattleEvent.OnPlayerReady, OnPlayerReady);
		EventManager.Instance.RegisterEvent (BattleEvent.OnBasicCommandSelected, OnBasicCommandSelected);
		EventManager.Instance.RegisterEvent (BattleEvent.OnCommandClicked, OnCommandClicked);
		EventManager.Instance.RegisterEvent (BattleEvent.OnCommandSelected, OnCommandSelected);
		EventManager.Instance.RegisterEvent (BattleEvent.OnCommandExecute, OnExecuteCommand);
		EventManager.Instance.RegisterEvent (BattleEvent.BattleObjectDied, OnBattleObjectDied);
		EventManager.Instance.RegisterEvent (BattleEvent.BattleObjectEscape, OnBattleObjectEscape);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnBattleEnter, OnBattleEnter);		
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnEnemySpawn, OnEnemySpawn);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnPlayerSpawn, OnPlayerSpawn);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnBattleEnter, OnBattleStart);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnPlayerReady, OnPlayerReady);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnBasicCommandSelected, OnBasicCommandSelected);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnCommandClicked, OnCommandClicked);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnCommandSelected, OnCommandSelected);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnCommandExecute, OnExecuteCommand);
		EventManager.Instance.UnRegisterEvent (BattleEvent.BattleObjectDied, OnBattleObjectDied);
		EventManager.Instance.UnRegisterEvent (BattleEvent.BattleObjectEscape, OnBattleObjectEscape);
	}

	void Update()
	{
		if(!isPaused && GlobalManager.Instance.gameStatus == GameStatus.Battle)//若游戏未开始、暂停，或正在处理命令队列，则跳过帧循环
		{
			if(readyQueue.Count > 0)
				StartCoroutine(HandleReadyQueue());
			else if(commandQueue.Count > 0)
				StartCoroutine(HandleCommandQueue());
			else
				EventManager.Instance.PostEvent (BattleEvent.OnTimelineUpdate);
		}
	}

	/*EVENT CALLBACK*/

	void OnBattleEnter(MessageEventArgs args)
	{		
		GlobalManager.Instance.gameStatus = GameStatus.Battle;
		isPaused = true;
		enemys = new List<Enemy>();
		players = new List<Player>();
		commandQueue = new Queue();//指令序列
		readyQueue = new Queue();//Ready序列
	}

	void OnEnemySpawn(MessageEventArgs args)
	{		
		BattleObject bo = args.GetMessage<BattleObject>("Object");
		enemys.Add(bo as Enemy);
	}

	void OnPlayerSpawn(MessageEventArgs args)
	{		
		BattleObject bo = args.GetMessage<BattleObject>("Object");
		players.Add(bo as Player);
	}

	void OnBattleStart(MessageEventArgs args)
	{
		isPaused = false;
	}

	void OnPlayerReady(MessageEventArgs args)
	{
		Player player = args.GetMessage<Player>("Player");
		readyQueue.Enqueue(player);
	}

	void OnBasicCommandSelected(MessageEventArgs args)
	{
		foreach(Enemy enemy in enemys)
		{
			enemy.GetComponent<BattleObjectUIEvent>().DisableClick();
		}
		foreach(Player player in players)
		{
			player.GetComponent<BattleObjectUIEvent>().DisableClick();
		}
	}

	void OnCommandClicked(MessageEventArgs args)
	{
		string commandName = args.GetMessage<string>("CommandName");
		currentCommand = currentPlayer.availableCommands.Find((Command cmd)=>{return cmd.commandName == commandName;});
		switch(currentCommand.targetType)
		{
		case TargetType.SingleEnemy:
			foreach(Enemy enemy in enemys)
			{
				enemy.GetComponent<BattleObjectUIEvent>().EnableClick();
			}
			break;
		case TargetType.SingleAlly:
			foreach(Player player in players)
			{
				player.GetComponent<BattleObjectUIEvent>().EnableClick();
			}
			break;
		case TargetType.OtherAlly:
			foreach(Player player in players)
			{
				if(player != currentPlayer)
				{
					player.GetComponent<BattleObjectUIEvent>().EnableClick();
				}
			}
			break;
		default:
			EventManager.Instance.PostEvent(BattleEvent.OnCommandSelected);
			break;
		}
	}

	void OnCommandSelected(MessageEventArgs args)
	{
		currentPlayer.commandToExecute = currentCommand;
		bool result = SkillHelper.FillCommandTarget(currentPlayer);
		if(!result)
		{
			if(args.ContainsMessage("Target"))
			{
				BattleObject bo = args.GetMessage<BattleObject>("Target");
				currentCommand.targetList.Add(bo);
			}
		}
		currentPlayer.GetComponent<BattleObjectUIEvent>().EndReady();
		currentPlayer.battleStatus = BattleStatus.Action;
		foreach(Enemy enemy in enemys)
		{
			enemy.GetComponent<BattleObjectUIEvent>().DisableClick();
		}
		foreach(Player player in players)
		{
			player.GetComponent<BattleObjectUIEvent>().DisableClick();
		}

		readyQueue.Dequeue();
	}

	void OnExecuteCommand(MessageEventArgs args)
	{
		StartCoroutine(WaitEveryOne(1));
	}

	void OnBattleObjectDied(MessageEventArgs args)
	{
		BattleObject bo = args.GetMessage<BattleObject>("Object");
		if(bo.isEnemy)
		{
			enemys.Remove((Enemy)bo);
			if(enemys.Count == 0)
			{
				EventManager.Instance.PostEvent(BattleEvent.OnBattleWin);
				
				StartCoroutine(FinishBattle());
			}
		}
		else
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

	public Player GetCurrentPlayer()
	{
		return currentPlayer;
	}

	public BattleObject GetARandomEnemy(BattleObject bo)
	{
		if (bo.isEnemy)
			return players[UnityEngine.Random.Range (0, players.Count)];
		else
			return enemys[UnityEngine.Random.Range (0, enemys.Count)];
	}

	public BattleObject GetARandomAlly(BattleObject bo)
	{
		if (bo.isEnemy)
			return enemys[UnityEngine.Random.Range (0, players.Count)];
		else
			return players[UnityEngine.Random.Range (0, enemys.Count)];
	}

	public List<BattleObject> GetAllEnemies(BattleObject bo)
	{
		if (bo.isEnemy)
			return new List<BattleObject>(players.ToArray());
		else
			return new List<BattleObject>(enemys.ToArray());
	}

	public List<BattleObject> GetAllAllies(BattleObject bo)
	{
		if (bo.isEnemy)
			return new List<BattleObject>(enemys.ToArray());
		else
			return new List<BattleObject>(players.ToArray());
	}

	public List<BattleObject> GetEveryone()
	{
		List<BattleObject> ret = new List<BattleObject>();
		ret.AddRange(players.ToArray());
		ret.AddRange(enemys.ToArray());
		return ret;
	}

	public List<BattleObject> GetEveryoneElse(BattleObject bo)
	{
		List<BattleObject> ret = GetEveryone();
		ret.Remove(bo);
		return ret;
	}

	public void AddToCommandQueue(Command cmd)
	{
		commandQueue.Enqueue (cmd);
	}

	public bool IsBattlePaused()
	{
		return isPaused;
	}

	IEnumerator HandleCommandQueue()
	{
		PauseEveryOne();
		for (int i = 0; i < commandQueue.Count; i++) 
		{
			Command cmd;
			try
			{
				cmd = commandQueue.Dequeue() as Command;
			}
			catch(InvalidOperationException ex)
			{
				Debug.LogWarning(ex.ToString());
				continue;
			}
			cmd.source.GetComponent<BattleObjectUIEvent>().BeginExecute();
			yield return cmd.OnExecute();
			cmd.source.GetComponent<BattleObjectUIEvent>().EndExecute();
		}
		ResumeEveryOne();
	}

	IEnumerator HandleReadyQueue()
	{
		PauseEveryOne();
		while(readyQueue.Count > 0)
		{
			EventManager.Instance.PostEvent(BattleEvent.OnCommandShowUp);
			currentPlayer = readyQueue.Peek() as Player;
			currentPlayer.GetComponent<BattleObjectUIEvent>().BeginReady();
			while(readyQueue.Contains(currentPlayer))
			{
				yield return 0;
			}
		}
		ResumeEveryOne();
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
		isPaused = true;
		yield return new WaitForSeconds(10);
		EventManager.Instance.PostEvent(BattleEvent.OnBattleFinish);
		SaveManager.Instance.SaveGame();

		MessageEventArgs arg = new MessageEventArgs();
		arg.AddMessage("WindowID", UIWindowID.MapWindow);
		EventManager.Instance.PostEvent(UIEvent.OpenUIWindow, arg);
	}

	void PauseEveryOne()
	{
		isPaused = true;
	}

	void ResumeEveryOne()
	{
		isPaused = false;
	}
}
