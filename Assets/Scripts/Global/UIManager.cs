using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {
	
/*
界面管理器
*/
	
	private static UIManager s_Instance;
	public UIManager() { s_Instance = this; }
	public static UIManager Instance { get { return s_Instance; } }

	private List<GameObject> windowStack = new List<GameObject>();

	void OnEnable() 
	{
		EventManager.Instance.RegisterEvent (UIEvent.OpenUIWindow, OpenUIWindow);
	}
	
	void OnDisable () 
	{
		EventManager.Instance.UnRegisterEvent (UIEvent.OpenUIWindow, OpenUIWindow);		
	}

	void OpenUIWindow(MessageEventArgs args)
	{
		UIWindowID windowID = args.GetMessage<UIWindowID>("WindowID");

		if(windowStack.Count > 0)
			windowStack[windowStack.Count - 1].SetActive(false);

		GameObject window = windowStack.Find((GameObject x)=>{return x.name.Contains(windowID.ToString());});
		if(window != null)
		{
			window.SetActive(true);
			windowStack.Remove(window);
			windowStack.Add(window);
		}
		else
		{
			window = GameObject.Instantiate(Resources.Load(GlobalDataStructure.PATH_UIPREFAB_WINDOW + windowID.ToString()) as GameObject);
			windowStack.Add(window);
		}
	}

}
