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
	GameObject commandPanel;
	GameObject timeLine;

	List<Enemy> enemys = new List<Enemy>();
	List<Player> players = new List<Player>();

	void Start()
	{
		enemyPanel = battleCanvas.transform.FindChild ("EnemyPanel").gameObject;
		infoPanel = battleCanvas.transform.FindChild("InfoPanel").gameObject;
		commandPanel = battleCanvas.transform.FindChild("CommandPanel").gameObject;
		timeLine = battleCanvas.transform.FindChild ("TimeLine").gameObject;
	}

	public void EnterBattle()
	{
		int[] enemyIDs = new int[3]{10,10,10};

		LoadGUI ();
		LoadBGM ();
		LoadPlayer ();

		StartCoroutine(LoadEnemy (enemyIDs));
	}

	void LoadGUI()
	{
		mapCanvas.gameObject.SetActive (false);
		battleCanvas.gameObject.SetActive (true);
		message.text = "";
	}

	void LoadBGM()
	{
		AudioManager.Instance.PlayBGM ("Music/Battle0" + Random.Range(1,4));
	}

	void LoadPlayer(bool man = true, bool girl = true)
	{
		if(man)
		{
			GameObject player = Instantiate(Resources.Load("Character/Character00")) as GameObject;
			player.transform.SetParent(infoPanel.transform);
			player.transform.localPosition = new Vector3(0,0,0);
			players.Add(player.GetComponent<Player>());

			GameObject avatar = Instantiate(Resources.Load("avatar")) as GameObject;
			avatar.transform.SetParent(timeLine.transform);
			player.GetComponent<Player>().SetAvatar(avatar);
		}
		if(girl)
		{
			GameObject player = Instantiate(Resources.Load("Character/Character01")) as GameObject;
			player.transform.SetParent(infoPanel.transform);
			player.transform.localPosition = new Vector3(0,0,0);
			players.Add(player.GetComponent<Player>());

			GameObject avatar = Instantiate(Resources.Load("avatar")) as GameObject;
			avatar.transform.SetParent(timeLine.transform);
			player.GetComponent<Player>().SetAvatar(avatar);
		}
		if(players.Count > 1)
		{
			players[0].transform.DOLocalMoveX(-200,0.5f);
			players[1].transform.DOLocalMoveX(200,0.5f);
		}			
	}

	IEnumerator LoadEnemy(int[] enemyIDs)
	{
		for(int i = 0; i < enemyIDs.Length; i++)
		{
			GameObject enemy = Instantiate(Resources.Load("Enemy/Enemy" + enemyIDs[i])) as GameObject;

			enemy.transform.SetParent(enemyPanel.transform);
			enemy.transform.localPosition = new Vector3(0,0,0);
			enemy.transform.DOShakeScale(1);
			enemys.Add(enemy.GetComponent<Enemy>());

			GameObject avatar = Instantiate(Resources.Load("avatar")) as GameObject;
			avatar.transform.SetParent(timeLine.transform);
			enemy.GetComponent<Enemy>().SetAvatar(avatar);

			AddMessage("怪物出现了！");
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
	}

	void AddMessage(string msg)
	{
		message.text += msg +"\n";
	}
}
