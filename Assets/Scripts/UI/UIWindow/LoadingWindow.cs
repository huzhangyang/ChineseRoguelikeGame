using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingWindow : MonoBehaviour {

	public Text tips;

	void Start () {
		tips.text = "Tips：击败BOSS卷毛咩，有可能掉落大量珍贵毛皮！";
		GlobalManager.Instance.gameStatus = GlobalManager.GameStatus.Loading;
		StartCoroutine (LoadScene ());
	}

	IEnumerator LoadScene()
	{
		DataManager.Instance.LoadAllData ();
		yield return new WaitForSeconds (1f);//这一行是为了防止乃们看不清Loading过程-_-
		AsyncOperation asyncOp = Application.LoadLevelAsync("GameScene");
		yield return asyncOp;
		Application.LoadLevel("GameScene");
	}

}
