using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingLogic : MonoBehaviour {

	public Text tips;

	void Start () {
		//tips.text = 
		StartCoroutine (LoadScene ());
	}

	IEnumerator LoadScene()
	{
		yield return new WaitForSeconds (1f);//这一行是为了防止乃们看不清Loading过程-_-
		AsyncOperation asyncOp = Application.LoadLevelAsync("GameScene");
		yield return asyncOp;
		Application.LoadLevel("GameScene");
	}

}
