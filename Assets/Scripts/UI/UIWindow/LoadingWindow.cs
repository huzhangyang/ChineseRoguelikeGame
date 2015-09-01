using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingWindow : MonoBehaviour {

	public Text tips;

	void Start () 
	{
		if(GlobalManager.Instance.gameStatus == GameStatus.StartNewGame)
		{
			DataManager.Instance.CreateNewSave();
		}
		else
		{
			DataManager.Instance.LoadSave();
		}			
		SetTip();
		StartCoroutine (LoadScene ());
	}

	void SetTip()
	{
		tips.text = "Tips：击败BOSS卷毛咩，有可能掉落大量珍贵毛皮！";
	}

	IEnumerator LoadScene()
	{
		GlobalManager.Instance.gameStatus = GameStatus.Loading;
		DataManager.Instance.LoadAllData ();
		yield return new WaitForSeconds (1f);//这一行是为了防止乃们看不清Loading过程-_-
		AsyncOperation asyncOp = Application.LoadLevelAsync("GameScene");
		yield return asyncOp;
		Application.LoadLevel("GameScene");
		GlobalManager.Instance.gameStatus = GameStatus.Map;
	}

}
