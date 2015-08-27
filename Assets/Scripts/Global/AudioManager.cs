using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

/*
音频管理器
*/
	
	private static AudioManager s_Instance;
	public AudioManager() { s_Instance = this; }
	public static AudioManager Instance { get { return s_Instance; } }

	public AudioSource BGM;
	public float volumeBGM
	{
		get
		{
			return BGM.volume;
		}
		set
		{
			BGM.volume = value;
		}
	}
	public float volumeSE;

	public void Start()
	{
		DontDestroyOnLoad (this.gameObject);
		BGM.loop = true;
		volumeBGM = 1;
		volumeSE = 1;
	}

	public void PlayBGM(string name)
	{
		AudioClip clip = Resources.Load ("Music/" + name, typeof(AudioClip)) as AudioClip;
		BGM.clip = clip;
		BGM.Play ();
	}

	public void PauseBGM()
	{
		BGM.Pause ();
	}

	public void UnPauseBGM()
	{
		BGM.UnPause ();
	}

	public void StopBGM()
	{
		BGM.Stop ();
	}

	public void PlaySE(string name, GameObject go = null)
	{
		if (go == null)
			go = this.gameObject;
		AudioClip clip = Resources.Load ("SE/" + name, typeof(AudioClip)) as AudioClip;
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volumeSE;
		source.Play();
		Destroy(source, clip.length);
	}
}
