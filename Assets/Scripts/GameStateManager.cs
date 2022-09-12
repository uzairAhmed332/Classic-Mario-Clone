using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameStateManager : MonoBehaviour {
	public bool spawnFromPoint;
	public int spawnPointIdx;
	public int spawnPipeIdx;

	public int marioSize;
	public int deaths; 
	public int coins;
	public int scores;
	public float timeElapsed; //Make it infinite
	public bool hurryUp; //purpose = make music faster

	public string sceneToLoad; // what scene to load after level start screen finishes?
	public bool timeup;

	void Awake () {
		if (FindObjectsOfType (GetType ()).Length == 1) {
			DontDestroyOnLoad (gameObject);
			ConfigNewGame ();
		} else {
			Destroy (gameObject);
		}
	}
	
	public void ResetSpawnPosition() {
		spawnFromPoint = true;
		spawnPointIdx = 0;
		spawnPipeIdx = 0;
	}

	public void SetSpawnPipe(int idx) {
		spawnFromPoint = false;
		spawnPipeIdx = idx;
	}

	public void ConfigNewGame() {
		marioSize = 0;
		deaths = 0;
		coins = 0;
		scores = 0;
		timeElapsed = 0.0f; 
		hurryUp = false;
		ResetSpawnPosition ();
		sceneToLoad = null;
		timeup = false;
	}

	public void ConfigNewLevel() {
		timeElapsed = 400.5f; //todo: Here add the previous time from last level
		hurryUp = false;
		ResetSpawnPosition ();
	}

	public void ConfigReplayedLevel() { // e.g. Mario respawns
		timeElapsed = 400.5f;
		hurryUp = false;
	}

	public void SaveGameState() { // also called before Mario dies
		LevelManager t_LevelManager = FindObjectOfType<LevelManager> ();
		marioSize = t_LevelManager.marioSize;
		deaths = t_LevelManager.deaths;
		coins = t_LevelManager.coins;
		scores = t_LevelManager.scores;
		timeElapsed = t_LevelManager.timeElapsed + LevelManager.loadSceneDelay;
		hurryUp = t_LevelManager.hurryUp;
	}

}
