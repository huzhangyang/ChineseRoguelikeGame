using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingPanel : MonoBehaviour {

	public Slider musicVolume;
	public Slider seVolume;

	void OnEnable()
	{
		musicVolume.value = DataManager.Instance.GetConfigData().volumeBGM;
		seVolume.value = DataManager.Instance.GetConfigData().volumeSE;
	}

	void OnDisable()
	{
		SaveManager.Instance.SaveConfig();
	}

	public void OnMusicVolumeChanged(float value)
	{
		DataManager.Instance.GetConfigData().volumeBGM = value;
	}

	public void OnSEVolumeChanged(float value)
	{
		DataManager.Instance.GetConfigData().volumeSE = value;
	}

	public void OnExitButtonClick()
	{
		MessageEventArgs arg = new MessageEventArgs();
		arg.AddMessage("WindowID", UIWindowID.IntroWindow);
		EventManager.Instance.PostEvent(UIEvent.OpenUIWindow, arg);
	}

}
