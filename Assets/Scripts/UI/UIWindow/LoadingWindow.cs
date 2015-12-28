using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingWindow : MonoBehaviour {

	public Text tips;

	void OnEnable() 
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
		StartCoroutine(LoadMainContent());
	}

	void SetTip()
	{
		tips.text = "Tips：击败BOSS卷毛咩，有可能掉落大量珍贵毛皮！";
	}

	IEnumerator LoadMainContent()
	{
		GlobalManager.Instance.gameStatus = GameStatus.Loading;
		DataManager.Instance.LoadAllData ();

		yield return new WaitForSeconds(1);

		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("WindowID", UIWindowID.MapWindow);
		EventManager.Instance.PostEvent(UIEvent.OpenUIWindow, args);

		GlobalManager.Instance.gameStatus = GameStatus.Map;
	}

}
