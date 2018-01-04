using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {
	static SoundManager instance = null;
	public static SoundManager Instance { get { return instance; } }

	AudioSource soundPlayer;

	float defaultVolume = 1.0f;

	static Dictionary<string, AudioClip> BGMs;
	static Dictionary<string, AudioClip> SEs;

	public void PlayBGM(string name){
		StartCoroutine (PlayBGMCoroutine (name));
	}
	IEnumerator PlayBGMCoroutine(string name){
		if (soundPlayer.clip == null) {
			PlayNewBGM (name);
			yield break;
		}
		if (soundPlayer.clip.name == name) {
			yield break;
		}

		float time = 0;
		const float FADETIME = 0.2f;

		float initialVolume = soundPlayer.volume;
		while (true) {
			time += Time.deltaTime;
			if (time > FADETIME) {
				soundPlayer.volume = 0;
				break;
			}
			soundPlayer.volume -= initialVolume * Time.deltaTime / FADETIME;
			yield return null;
		}
		PlayNewBGM (name);
	}
	void PlayNewBGM(string name){
		if (name == "None") {
			soundPlayer.clip = null;
		} else {
			soundPlayer.clip = BGMs [name];
			soundPlayer.volume = defaultVolume;
			soundPlayer.Play ();
		}
	}
	public void StopBGM(){
		soundPlayer.Stop ();
	}
	public void ReplayBGM(){
		soundPlayer.Play();
	}
	public void EndBGM(){
		PlayBGM ("None");
	}
	public void PlaySE(string name){
		soundPlayer.PlayOneShot (SEs[name]);
		//soundPlayer.PlayOneShot (SEs[name], Configuration.soundEffectVolume);
	}

	/*public void ChangeBGMVolume(float BGMVolume) {
		soundPlayer.volume = Configuration.BGMVolume;
	}*/

	void Awake () {
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		}
		else {
			instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
		LoadBGMsAndSEs ();
		soundPlayer = gameObject.GetComponent<AudioSource>();
	}

	static void LoadBGMsAndSEs(){
		AudioClip[] BGMLists = Resources.LoadAll<AudioClip> ("BGMs");
		BGMs = new Dictionary<string, AudioClip> ();
		foreach (AudioClip BGM in BGMLists) {
			BGMs [BGM.name] = BGM;
		}
		AudioClip[] SELists = Resources.LoadAll<AudioClip> ("SEs");
		SEs = new Dictionary<string, AudioClip> ();
		foreach (AudioClip SE in SELists) {
			SEs [SE.name] = SE;
		}
	}
}