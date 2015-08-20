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
		if(args.ContainMessage("Man"))
		{
			GameObject player = Instantiate(Resources.Load(GlobalDataStructure.PATH_BATTLE + "Character00")) as GameObject;
			player.transform.SetParent(infoPanel.transform);
			player.transform.localScale = new Vector3(1,1,1);
			player.transform.localPosition = new Vector3(0,0,0);
			BattleLogic.players.Add(player.GetComponent<Player>());
			
			GameObject avatar = Instantiate(Resources.Load("UI/TimelineAvatar")) as GameObject;
			avatar.transform.SetParent(timeLine.transform);
			player.GetComponent<Player>().SetAvatar(avatar);

			if(args.ContainMessage("Girl"))
			{
				player.transform.DOLocalMoveX(-200,0.5f);
			}
		}
		if(args.ContainMessage("Girl"))
		{
			GameObject player = Instantiate(Resources.Load(GlobalDataStructure.PATH_BATTLE + "Character01")) as GameObject;
			player.transform.SetParent(infoPanel.transform);
			player.transform.localScale = new Vector3(1,1,1);
			player.transform.localPosition = new Vector3(0,0,0);
			BattleLogic.players.Add(player.GetComponent<Player>());
			
			GameObject avatar = Instantiate(Resources.Load("UI/TimelineAvatar")) as GameObject;
			avatar.transform.SetParent(timeLine.transform);
			player.GetComponent<Player>().SetAvatar(avatar);

			if(args.ContainMessage("Man"))
			{
				player.transform.DOLocalMoveX(200,0.5f);
			}
		}

		if(args.ContainMessage("Enemy"))
		{
			string[] enemyIDs = args.GetMessage("Enemy").Split(',');
			StartCoroutine(LoadEnemy(enemyIDs));
		}
	}

	void OnPlayerReady(MessageEventArgs args)
	{
		commandPanel.SetActive(true);
	}

	void OnShowAvailableCommands(MessageEventArgs args)
	{
		subCommandPanel.SetActive (true);
		foreach(Transform child in subCommandPanel.transform.FindChild("SubCommandButtonPanel"))
		{
			Destroy(child.gameObject);
		}
		int commandCount = Convert.ToInt32(args.GetMessage ("CommandCount"));
		for(int i = 0 ; i < commandCount; i++)
		{
			string commandType = args.GetMessage ("Command" + i +"Type");
			string commandName = args.GetMessage ("Command" + i +"Name");
			string commandDescription = args.GetMessage ("Command" + i +"Description");

			GameObject commandButton = Instantiate(Resources.Load("UI/CommandButton")) as GameObject;
			commandButton.transform.SetParent(subCommandPanel.transform.FindChild("SubCommandButtonPanel"));
			commandButton.transform.localScale = new Vector3(1,1,1);
			commandButton.GetComponent<CommandButton>().Init(commandType, commandName, commandDescription);
		}
	}

	void OnSelectCommand(MessageEventArgs args)
	{
		commandPanel.SetActive(false);
		subCommandPanel.SetActive (false);
	}

	IEnumerator LoadEnemy(string[] enemyIDs)
	{
		for(int i = 0; i < enemyIDs.Length; i++)
		{
			string loadEnemyPath = GlobalDataStructure.PATH_BATTLE + "Enemy";
			if(Convert.ToInt32(enemyIDs[i]) < 10) loadEnemyPath += "0";
			GameObject enemy = Instantiate(Resources.Load(loadEnemyPath + enemyIDs[i])) as GameObject;
			
			enemy.transform.SetParent(enemyPanel.transform);
			enemy.transform.localScale = new Vector3(1,1,1);
			enemy.transform.localPosition = new Vector3(0, -50 , 0);
			BattleLogic.enemys.Add(enemy.GetComponent<Enemy>());
			
			GameObject avatar = Instantiate(Resources.Load("UI/TimelineAvatar")) as GameObject;
			avatar.transform.SetParent(timeLine.transform);
			enemy.GetComponent<Enemy>().SetAvatar(avatar);

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
				else if(i == 2)
					enemy.transform.DOLocalMoveX(200,0.5f);
				break;
			}
		}
		yield return new WaitForSeconds(1f);
		EventManager.Instance.PostEvent (EventDefine.StartBattle);
	}
}
