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
	Transform commandButtonPanel;
	GameObject timeLine;

	void Awake () 
	{
		enemyPanel = this.transform.FindChild("EnemyPanel").gameObject;
		playerPanel = this.transform.FindChild("PlayerPanel").gameObject;
		commandPanel = this.transform.FindChild("CommandPanel").gameObject;
		subCommandPanel = this.transform.FindChild("SubCommandPanel").gameObject;
		commandButtonPanel =  subCommandPanel.transform.FindChild("SubCommandButton").FindChild("Panel");
		timeLine = this.transform.FindChild("TimeLine").gameObject;
	}
	
	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (BattleEvent.OnBattleEnter, OnEnterBattle);
		EventManager.Instance.RegisterEvent (BattleEvent.OnCommandShowUp, OnCommandShowUp);
		EventManager.Instance.RegisterEvent (BattleEvent.OnBasicCommandSelected, OnBasicCommandSelected);
		EventManager.Instance.RegisterEvent (BattleEvent.OnCommandSelected, OnCommandSelected);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnBattleEnter, OnEnterBattle);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnCommandShowUp, OnCommandShowUp);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnBasicCommandSelected, OnBasicCommandSelected);
		EventManager.Instance.UnRegisterEvent (BattleEvent.OnCommandSelected, OnCommandSelected);
	}

	public void SelectBasicCommand(int commandID)
	{
		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("CommandID", commandID);
		EventManager.Instance.PostEvent(BattleEvent.OnBasicCommandSelected, args);
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

	void OnCommandShowUp(MessageEventArgs args)
	{
		commandPanel.SetActive(true);
		subCommandPanel.SetActive (true);
		commandPanel.GetComponent<CommandPanelUIEvent>().SetButtonActive();
	}

	void OnBasicCommandSelected(MessageEventArgs args)
	{
		Player player = BattleManager.Instance.GetCurrentPlayer();
		foreach(Transform child in commandButtonPanel)
		{
			Destroy(child.gameObject);
		}

		CommandType commandType = (CommandType)args.GetMessage<int>("CommandID");
		List<Command> validCommands = player.availableCommands.FindAll((x)=>{return x.commandType == commandType;});
		for(int i = 0 ; i < validCommands.Count; i++)
		{
			GameObject commandButton = Instantiate(Resources.Load(GlobalDataStructure.PATH_UIPREFAB_BATTLE + "CommandButton")) as GameObject;
			commandButton.transform.SetParent(commandButtonPanel, false);
			commandButton.GetComponent<CommandButtonUIEvent>().Init(validCommands[i], validCommands[i].IsAvailable());
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
			GameObject player = Instantiate(Resources.Load(GlobalDataStructure.PATH_UIPREFAB_BATTLE + "Character")) as GameObject;
			player.transform.SetParent(playerPanel.transform, false);
			player.GetComponent<Player>().Init(0);			
			
			player.transform.DOLocalMoveX(-250,0.5f);
		}

		if(args.ContainsMessage("Girl"))
		{
			GameObject player = Instantiate(Resources.Load(GlobalDataStructure.PATH_UIPREFAB_BATTLE + "Character")) as GameObject;
			player.transform.SetParent(playerPanel.transform, false);
			player.GetComponent<Player>().Init(1);
			
			player.transform.DOLocalMoveX(250,0.5f);
		}

		if(args.ContainsMessage("Enemy"))
		{
			int[] enemyIDs = args.GetMessage<int[]>("Enemy");
			for(int i = 0; i < enemyIDs.Length; i++)
			{				
				GameObject enemy = Instantiate(Resources.Load(GlobalDataStructure.PATH_UIPREFAB_BATTLE + "Enemy")) as GameObject;			
				enemy.transform.SetParent(enemyPanel.transform, false);
				enemy.GetComponent<Enemy>().Init(enemyIDs[i]);
				enemy.transform.localScale = new Vector3(0,0,0);
				enemy.transform.DOScale(1, 1);
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

		yield return new WaitForSeconds(0.5f);
		EventManager.Instance.PostEvent (BattleEvent.OnBattleStart);
	}
}
