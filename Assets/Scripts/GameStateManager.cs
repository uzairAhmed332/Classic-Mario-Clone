using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public bool delayWhenGamestatesaved = false;

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

	public void ConfigNewLevel() { //Make everything resetwhen new level starts
		marioSize = 0;
		deaths = 0;
		coins = 0;
		scores = 0;
		timeElapsed = 0.0f; //todo: Here add the previous time from last level
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
		timeElapsed = t_LevelManager.timeElapsed; // "+ LevelManager.loadSceneDelay" LevelManager.loadSceneDelay is added to add the (extra)time to read feedback!
		hurryUp = t_LevelManager.hurryUp;

		//delayWhenGamestatesaved = true; getting true from LevelStartScreen when mario dies!
		Invoke("SetBoolBack", 5f);
	}

	private void SetBoolBack()
	{
		delayWhenGamestatesaved = false;
	}


	public void savePerformanceInFile()
	{
		Debug.Log("***Saving Performace Matrics***");
		LevelManager t_LevelManager = FindObjectOfType<LevelManager>();
		//Path of the file
		string path = Application.dataPath + "/PerformaceMetrics.txt";
		//Create File if it doesn't exist
		if (!File.Exists(path))
		{
			File.WriteAllText(path, "Performance Metric \n\n");
		}
        //Content of the file 
        //Call When 
        //1. Everytime Level ends
        //2. After 15 min when level ends (Think about it wheter to do it or not! I think not)
        string content = "Level: " + SceneManager.GetActiveScene().name + "\n" +
                         "Score: " + t_LevelManager.scores + "\n" +
                         "Deaths: " + t_LevelManager.deaths + "\n" +
                         "Time: " + t_LevelManager.timeElapsed + "\n\n";
        //Add some to text to it
        File.AppendAllText(path, content);
	}

}
