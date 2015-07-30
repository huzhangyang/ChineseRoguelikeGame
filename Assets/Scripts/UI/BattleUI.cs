using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUI : MonoBehaviour {

	public Canvas battleCanvas;
	public Canvas mapCanvas;
	public Text message;
	GameObject enemyPanel;
	GameObject infoPanel;
	GameObject commandPanel;
	GameObject timeLine;

	void Start () 
	{
		enemyPanel = battleCanvas.transform.FindChild ("EnemyPanel").gameObject;
		infoPanel = battleCanvas.transform.FindChild("InfoPanel").gameObject;
		commandPanel = battleCanvas.transform.FindChild("CommandPanel").gameObject;
		timeLine = battleCanvas.transform.FindChild ("TimeLine").gameObject;
	}
	
	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (EventDefine.EnterBattle, LoadGUI);
		EventManager.Instance.RegisterEvent (EventDefine.UpdateTimeline, UpdateTimeline);

	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (EventDefine.EnterBattle, LoadGUI);
		EventManager.Instance.UnRegisterEvent (EventDefine.UpdateTimeline, UpdateTimeline);
	}

	void LoadGUI(MessageEventArgs args)
	{
		mapCanvas.gameObject.SetActive (false);
		battleCanvas.gameObject.SetActive (true);
		message.text = "";

		if(args.ContainMessage("man"))
		{
			GameObject player = Instantiate(Resources.Load("Character/Character00")) as GameObject;
			player.transform.SetParent(infoPanel.transform);
			player.transform.localPosition = new Vector3(0,0,0);
			BattleLogic.players.Add(player.GetComponent<Player>());
			
			GameObject avatar = Instantiate(Resources.Load("avatar")) as GameObject;
			avatar.transform.SetParent(timeLine.transform);
			player.GetComponent<Player>().SetAvatar(avatar);

			if(args.ContainMessage("girl"))
			{
				player.transform.DOLocalMoveX(-200,0.5f);
			}
		}
		if(args.ContainMessage("girl"))
		{
			GameObject player = Instantiate(Resources.Load("Character/Character01")) as GameObject;
			player.transform.SetParent(infoPanel.transform);
			player.transform.localPosition = new Vector3(0,0,0);
			BattleLogic.players.Add(player.GetComponent<Player>());
			
			GameObject avatar = Instantiate(Resources.Load("avatar")) as GameObject;
			avatar.transform.SetParent(timeLine.transform);
			player.GetComponent<Player>().SetAvatar(avatar);

			if(args.ContainMessage("man"))
			{
				player.transform.DOLocalMoveX(200,0.5f);
			}
		}

		if(args.ContainMessage("enemy"))
		{
			string[] enemyIDs = args.GetMessage("enemy").Split(',');
			StartCoroutine(LoadEnemy(enemyIDs));
		}
	}

	IEnumerator LoadEnemy(string[] enemyIDs)
	{
		for(int i = 0; i < enemyIDs.Length; i++)
		{
			GameObject enemy = Instantiate(Resources.Load("Enemy/Enemy" + enemyIDs[i])) as GameObject;
			
			enemy.transform.SetParent(enemyPanel.transform);
			enemy.transform.localPosition = new Vector3(0,0,0);
			BattleLogic.enemys.Add(enemy.GetComponent<Enemy>());
			
			GameObject avatar = Instantiate(Resources.Load("avatar")) as GameObject;
			avatar.transform.SetParent(timeLine.transform);
			enemy.GetComponent<Enemy>().SetAvatar(avatar);
			
			AddMessage("怪物出现了！");
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
		EventManager.Instance.PostEvent (EventDefine.StartBattle, new MessageEventArgs ());
	}

	void UpdateTimeline(MessageEventArgs args)
	{
		foreach(Player player in BattleLogic.players)
		{
			if(player.battleStatus == BattleObject.BattleStatus.Prepare)
				player.timelinePosition += 10;
			if(player.timelinePosition == 500)
			{
				player.battleStatus = BattleObject.BattleStatus.Ready;
				commandPanel.SetActive(true);
			}
		}
	}

	public void AddMessage(string msg)
	{
		message.text += msg +"\n";
	}
}
