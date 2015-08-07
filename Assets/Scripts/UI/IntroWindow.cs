using UnityEngine;
using System.Collections;

public class IntroWindow : MonoBehaviour {

	void Start () 
	{
		AudioManager.Instance.PlayBGM ("Music/Theme");
	}

	public void OnButtonNew()
	{
		Application.LoadLevel("LoadingScene");
	}
	
	public void OnButtonContinue()
	{
		
	}
	
	public void OnButtonSetting()
	{
		
	}
	
	public void OnButtonExit()
	{
		Application.Quit();
	}

}
