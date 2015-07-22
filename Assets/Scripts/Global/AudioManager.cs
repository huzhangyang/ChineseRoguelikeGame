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
	public float volumeBGM;
	public float volumeSE;

	public void Start()
	{
		BGM.loop = true;
		BGM.Play ();
	}

	public void PlayBGM(string path)
	{
		AudioClip clip = AudioClip.Create(path, 44100 * 2, 1, 44100, true);
		BGM.clip = clip;
		BGM.volume = volumeBGM;
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

	public void PlaySE(string path, GameObject go = null)
	{
		if (go == null)
			go = this.gameObject;
		AudioClip clip = AudioClip.Create(path, 44100 * 2, 1, 44100, true);
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volumeSE;
		source.Play();
		Destroy(source, clip.length);
	}
}
