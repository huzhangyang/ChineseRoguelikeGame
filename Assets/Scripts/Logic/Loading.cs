using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Loading : MonoBehaviour {

	public Slider ProgressBar;
	// Use this for initialization
	void Start () {
		StartCoroutine (LoadScene ());
	}

	IEnumerator LoadScene()
	{
		yield return new WaitForSeconds (1f);//这一行是为了防止乃们看不清Loading过程-_-
		AsyncOperation asyncOp = Application.LoadLevelAsync("GameScene");
		ProgressBar.value = asyncOp.progress;
		yield return asyncOp;
		Application.LoadLevel("GameScene");
	}

}
