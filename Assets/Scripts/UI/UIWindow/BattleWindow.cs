using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class BattleWindow: MonoBehaviour {

	GameObject enemyPanel;
	GameObject infoPanel;
	GameObject commandPanel;
	GameObject subCommandPanel;
	GameObject timeLine;

	void Awake () 
	{
		enemyPanel = this.transform.FindChild ("EnemyPanel").gameObject;
		infoPanel = this.transform.FindChild("InfoPanel").gameObject;
		commandPanel = this.transform.FindChild("CommandPanel").gameObject;
		subCommandPanel = this.transform.FindChild("SubCommandPanel").gameObject;
		timeLine = this.transform.FindChild ("TimeLine").gameObject;
	}
	
	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (EventDefine.EnterBattle, OnEnterBattle);
		EventManager.Instance.RegisterEvent (EventDefine.PlayerReady, OnPlayerReady);
		EventManager.Instance.RegisterEvent (EventDefine.ShowAvailableCommands, OnShowAvailableCommands);
		EventManager.Instance.RegisterEvent (EventDefine.SelectCommand, OnSelectCommand);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (EventDefine.EnterBattle, OnEnterBattle);
		EventManager.Instance.UnRegisterEvent (EventDefine.PlayerReady, OnPlayerReady);
		EventManager.Instance.UnRegisterEvent (EventDefine.ShowAvailableCommands, OnShowAvailableCommands);
		EventManager.Instance.UnRegisterEvent (EventDefine.SelectCommand, OnSelectCommand);
	}

	void OnEnterBattle(MessageEventArgs args)
	{
		foreach(Transform child in enemyPanel.transform)
		{
			Destroy(child.gameObject);
		}
		foreach(Transform child in infoPanel.transform)
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
	}

	void OnShowAvailableCommands(MessageEventArgs args)
	{
		subCommandPanel.SetActive (true);
		subCommandPanel.transform.FindChild("CommandDescription").GetComponent<Text>().text = "";
		foreach(Transform child in subCommandPanel.transform.FindChild("SubCommandButtonPanel"))
		{
			Destroy(child.gameObject);
		}

		var commands = BattleLogic.GetCurrentPlayer().availableCommands;
		for(int i = 0 ; i < commands.Count; i++)
		{
			GameObject commandButton = Instantiate(Resources.Load("UI/CommandButton")) as GameObject;
			commandButton.transform.SetParent(subCommandPanel.transform.FindChild("SubCommandButtonPanel"), false);
			commandButton.GetComponent<CommandButtonUIEvent>().Init(commands[i].commandID, commands[i].commandName, commands[i].commandDescription);
		}
	}

	void OnSelectCommand(MessageEventArgs args)
	{
		commandPanel.SetActive(false);
		subCommandPanel.SetActive (false);
	}

	IEnumerator StartBattle(MessageEventArgs args)
	{
		if(args.ContainMessage("Man"))
		{
			GameObject player = Instantiate(Resources.Load(GlobalDataStructure.PATH_BATTLE + "Character00")) as GameObject;
			player.transform.SetParent(infoPanel.transform, false);
			BattleLogic.players.Add(player.GetComponent<Player>());
			
			GameObject avatar = Instantiate(Resources.Load("UI/TimelineAvatar")) as GameObject;
			avatar.transform.SetParent(timeLine.transform, false);
			player.GetComponent<BattleObjectUIEvent>().SetAvatar(avatar);
			
			if(args.ContainMessage("Girl"))
			{
				player.transform.DOLocalMoveX(-200,0.5f);
			}
		}

		if(args.ContainMessage("Girl"))
		{
			GameObject player = Instantiate(Resources.Load(GlobalDataStructure.PATH_BATTLE + "Character01")) as GameObject;
			player.transform.SetParent(infoPanel.transform, false);
			BattleLogic.players.Add(player.GetComponent<Player>());
			
			GameObject avatar = Instantiate(Resources.Load("UI/TimelineAvatar")) as GameObject;
			avatar.transform.SetParent(timeLine.transform, false);
			player.GetComponent<BattleObjectUIEvent>().SetAvatar(avatar);
			
			if(args.ContainMessage("Man"))
			{
				player.transform.DOLocalMoveX(200,0.5f);
			}
		}

		if(args.ContainMessage("Enemy"))
		{
			string[] enemyIDs = args.GetMessage("Enemy").Split(',');
			for(int i = 0; i < enemyIDs.Length; i++)
			{
				string loadEnemyPath = GlobalDataStructure.PATH_BATTLE + "Enemy";
				if(Convert.ToInt32(enemyIDs[i]) < 10) loadEnemyPath += "0";
				
				GameObject enemy = Instantiate(Resources.Load(loadEnemyPath + enemyIDs[i])) as GameObject;			
				enemy.transform.SetParent(enemyPanel.transform, false);
				BattleLogic.enemys.Add(enemy.GetComponent<Enemy>());
				
				GameObject avatar = Instantiate(Resources.Load("UI/TimelineAvatar")) as GameObject;
				avatar.transform.SetParent(timeLine.transform, false);
				enemy.GetComponent<BattleObjectUIEvent>().SetAvatar(avatar);
				
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

		EventManager.Instance.PostEvent (EventDefine.StartBattle);
	}
}
