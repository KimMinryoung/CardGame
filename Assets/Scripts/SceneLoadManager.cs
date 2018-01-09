using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour {
	void Start(){
		if (!GameData.gameStarted) {
			GameData.gameStarted = true;
			nextSceneName = "Title";
			StartCoroutine (LoadScene ());
		}
	}

	string nextSceneName;
	IEnumerator LoadScene(){
		AsyncOperation op = SceneManager.LoadSceneAsync (nextSceneName);
		yield return null;
	}
	/*public static void LoadScene(string sceneName){
		nextSceneName = sceneName;
		SceneManager.LoadScene ("LoadingScene");
	}*/
}
