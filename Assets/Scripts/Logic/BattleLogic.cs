using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class BattleLogic : MonoBehaviour {

	public Canvas battleCanvas;
	public Canvas mapCanvas;
	public Text message;
	GameObject enemyPanel;
	GameObject infoPanel;

	List<GameObject> enemys = new List<GameObject>();
	List<GameObject> players = new List<GameObject>();

	void Start()
	{
		enemyPanel = battleCanvas.transform.FindChild ("EnemyPanel").gameObject;
		infoPanel = battleCanvas.transform.FindChild("InfoPanel").gameObject;
	}

	public void EnterBattle()
	{
		int[] enemyIDs = new int[3]{10,10,10};

		LoadGUI ();
		LoadEnemy (enemyIDs);
		LoadPlayer ();
		LoadBGM ();
	}

	void LoadGUI()
	{
		mapCanvas.gameObject.SetActive (false);
		battleCanvas.gameObject.SetActive (true);
		message.text = "";
	}

	void LoadEnemy(int[] enemyIDs)
	{
		for(int i = 0; i < enemyIDs.Length; i++)
		{
			GameObject enemy = Instantiate(Resources.Load("Enemy/Enemy" + enemyIDs[i])) as GameObject;
			enemy.transform.SetParent(enemyPanel.transform);
			enemy.transform.localPosition = new Vector3(0,0,0);
			enemys.Add(enemy);
			AddMessage("怪物出现了！");
		}
		switch(enemys.Count)
		{
			case 2:	enemys[0].transform.DOLocalMoveX(-150,0.5f);
					enemys[1].transform.DOLocalMoveX(150,0.5f);
					break;
			case 3: enemys[1].transform.DOLocalMoveX(-200,0.5f);
					enemys[2].transform.DOLocalMoveX(200,0.5f);
					break;
		}
	}

	void LoadPlayer(bool man = true, bool girl = true)
	{
		if(man)
		{
			GameObject player = Instantiate(Resources.Load("Character/Character00")) as GameObject;
			player.transform.SetParent(infoPanel.transform);
			player.transform.localPosition = new Vector3(0,0,0);
			players.Add(player);
		}
		if(girl)
		{
			GameObject player = Instantiate(Resources.Load("Character/Character01")) as GameObject;
			player.transform.SetParent(infoPanel.transform);
			player.transform.localPosition = new Vector3(0,0,0);
			players.Add(player);
		}
		if(players.Count > 1)
		{
			players[0].transform.DOLocalMoveX(-200,0.5f);
			players[1].transform.DOLocalMoveX(200,0.5f);
		}			
	}
	
	void LoadBGM()
	{
		AudioManager.Instance.PlayBGM ("Music/Battle0" + Random.Range(1,4));
	}

	void AddMessage(string msg)
	{
		message.text += msg +"\n";
	}
}
