using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class BattleWindow: MonoBehaviour {

	GameObject enemyPanel;
	GameObject playerPanel;
	GameObject commandPanel;
	GameObject subCommandPanel;
	GameObject commandDescrpition;
	Transform commandButtonPanel;
	GameObject timeLine;

	void Awake () 
	{
		enemyPanel = this.transform.FindChild("EnemyPanel").gameObject;
		playerPanel = this.transform.FindChild("PlayerPanel").gameObject;
		commandPanel = this.transform.FindChild("CommandPanel").gameObject;
		subCommandPanel = this.transform.FindChild("SubCommandPanel").gameObject;
		commandDescrpition = subCommandPanel.transform.FindChild("CommandDescription").gameObject;
		commandButtonPanel =  subCommandPanel.transform.FindChild("SubCommandButton").FindChild("Panel");
		timeLine = this.transform.FindChild("TimeLine").gameObject;
	}
	
	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (BattleEvent.OnBattleEnter, OnEnterBattle);
		EventManager.Instance.RegisterEvent (BattleEvent.OnPlayerReady, OnPlayerReady);
		EventManager.Instance.RegisterEvent (BattleEvent.OnBasicCommandSelected, OnBasicCommandSelected);
		EventManager.Instance.RegisterEvent (BattleEvent.OnCommandSelected, OnCommandSelected);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnBattleEnter, OnEnterBattle);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnPlayerReady, OnPlayerReady);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnBasicCommandSelected, OnBasicCommandSelected);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnCommandSelected, OnCommandSelected);
	}

	void OnEnterBattle(MessageEventArgs args)
	{
		foreach(Transform child in enemyPanel.transform)
		{
			Destroy(child.gameObject);
		}
		foreach(Transform child in playerPanel.transform)
		{
			Destroy(child.gameObject);
		}
		foreach(Transform child in timeLine.transform)
		{
			Destroy(child.gameObject);
		}

		StartCoroutine(StartBattle(args));
	}

	void OnPlayerReady(MessageEventArgs args)
	{
		commandPanel.SetActive(true);
		subCommandPanel.SetActive (true);
		string playerName = args.GetMessage<string>("PlayerName");
		commandDescrpition.GetComponent<Text>().text = playerName + "如何决策？";
	}

	void OnBasicCommandSelected(MessageEventArgs args)
	{
		string playerName = args.GetMessage<string>("PlayerName");
		commandDescrpition.GetComponent<Text>().text = playerName + "如何决策？";
		commandButtonPanel.DOLocalMoveX(-360, 0.2f);
		foreach(Transform child in commandButtonPanel)
		{
			Destroy(child.gameObject);
		}

		var commands = BattleLogic.GetCurrentPlayer().availableCommands;
		for(int i = 0 ; i < commands.Count; i++)
		{
			GameObject commandButton = Instantiate(Resources.Load("Battle/CommandButton")) as GameObject;
			commandButton.transform.SetParent(commandButtonPanel, false);
			commandButton.GetComponent<CommandButtonUIEvent>().Init(commands[i].commandName, commands[i].commandDescription);
		}
	}

	void OnCommandSelected(MessageEventArgs args)
	{
		commandPanel.SetActive(false);
		subCommandPanel.SetActive(false);
		foreach(Transform child in commandButtonPanel)
		{
			Destroy(child.gameObject);
		}
	}

	IEnumerator StartBattle(MessageEventArgs args)
	{
		if(args.ContainsMessage("Man"))
		{
			GameObject player = Instantiate(Resources.Load(GlobalDataStructure.PATH_BATTLE + "Character")) as GameObject;
			player.transform.SetParent(playerPanel.transform, false);
			player.GetComponent<Player>().Init(0);			
			
			if(args.ContainsMessage("Girl"))
			{
				player.transform.DOLocalMoveX(-200,0.5f);
			}
		}

		if(args.ContainsMessage("Girl"))
		{
			GameObject player = Instantiate(Resources.Load(GlobalDataStructure.PATH_BATTLE + "Character")) as GameObject;
			player.transform.SetParent(playerPanel.transform, false);
			player.GetComponent<Player>().Init(1);
			

			
			if(args.ContainsMessage("Man"))
			{
				player.transform.DOLocalMoveX(200,0.5f);
			}
		}

		if(args.ContainsMessage("Enemy"))
		{
			int[] enemyIDs = args.GetMessage<int[]>("Enemy");
			for(int i = 0; i < enemyIDs.Length; i++)
			{				
				GameObject enemy = Instantiate(Resources.Load(GlobalDataStructure.PATH_BATTLE + "Enemy")) as GameObject;			
				enemy.transform.SetParent(enemyPanel.transform, false);
				enemy.GetComponent<Enemy>().Init(enemyIDs[i]);
				
				enemy.transform.DOShakeScale(1);
				yield return new WaitForSeconds(1f);
				switch(enemyIDs.Length)
				{
				case 2: if(i == 0)
						enemy.transform.DOLocalMoveX(-150,0.5f);
					else if(i == 1)
						enemy.transform.DOLocalMoveX(150,0.5f);
					break;
				case 3:	if(i == 0)
						enemy.transform.DOLocalMoveX(-200,0.5f);
					else if(i == 1)
						enemy.transform.DOLocalMoveX(200,0.5f);
					break;
				}
			}
		}

		EventManager.Instance.PostEvent (BattleEvent.OnBattleStart);
	}
}
