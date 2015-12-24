using UnityEngine;
using System.Collections;

public class GlobalManager : MonoBehaviour {

/*
全局管理器
*/

	private static GlobalManager s_Instance;
	public GlobalManager() { s_Instance = this; }
	public static GlobalManager Instance { get { return s_Instance; } }

	public GameStatus gameStatus = GameStatus.Title;
	
	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		Application.runInBackground = false;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		DataManager.Instance.LoadConfig();

		MessageEventArgs args = new MessageEventArgs();
		args.AddMessage("WindowID", UIWindowID.IntroWindow);
		EventManager.Instance.PostEvent(UIEvent.OpenUIWindow, args);
	}
	
	void OnApplicationFocus(bool focusd)
	{
		if (!focusd) 
		{//进入后台

		}
		else
		{//进入前台

		}
	}
}

public enum GameStatus
{
	Title,
	StartNewGame,
	Loading,
	Map,
	Battle,
	Pause
}
