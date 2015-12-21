using UnityEngine;
using System.Collections;

[System.Serializable]
public class ConfigData{
	
/*
 * 设置选项类。保存全部的设置选项。
*/
	public float volumeBGM
	{
		get
		{
			return AudioManager.Instance.volumeBGM;
		}
		set
		{
			AudioManager.Instance.volumeBGM = value;
		}
	}
	public float volumeSE
	{
		get
		{
			return AudioManager.Instance.volumeSE;
		}
		set
		{
			AudioManager.Instance.volumeSE = value;
		}
	}

	public int currentLeaderID;
}
