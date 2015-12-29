using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

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
		{
			GameObject lastWindow = windowStack[windowStack.Count - 1];
			CanvasGroup canvasGroup = lastWindow.GetComponent<CanvasGroup>();
			canvasGroup.alpha = 1;
			canvasGroup.DOFade(0, 0.25f).OnComplete(()=>{lastWindow.SetActive(false);});
		}			

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

		if(windowStack.Count > 1)
		{
			CanvasGroup canvasGroup = window.GetComponent<CanvasGroup>();
			canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, 0.5f);
		}
	}

	public void PreloadWindow()
	{
		string[] windowIDs = Enum.GetNames(typeof(UIWindowID));
		for(int i = 0; i < windowIDs.Length; i++)
		{
			GameObject window = windowStack.Find((GameObject x)=>{return x.name.Contains(windowIDs[i]);});
			if(window == null)
			{
				window = GameObject.Instantiate(Resources.Load(GlobalDataStructure.PATH_UIPREFAB_WINDOW + windowIDs[i]) as GameObject);
				window.SetActive(false);
				windowStack.Add(window);
			}
		}
	}

}
