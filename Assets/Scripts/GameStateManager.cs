﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {
	public bool levelEndsCheck;
	public bool spawnFromPoint;
	public int spawnPointIdx;
	public int spawnPipeIdx;

	public Canvas traningTimeOverCanvas;
	public int marioSize;
	public int deaths; 
	public int coins;
	public int scores;
	public float timeElapsed; //Make it infinite
	public bool hurryUp; //purpose = make music faster

	private int timeTraning;
	private int timeTraningInt;
	private string timeTraningString;

	public string sceneToLoad; // what scene to load after level start screen finishes?
	public bool timeup;

    public bool dontShowDelayedFeedbackWhenDied = false; //if delayed feedback is true. Then dont show it when mario dies, Only show after level ends


	void Awake()
	{
		Debug.Log("LOL"+ Application.dataPath);
		if (!Constants.isnormalGamePlayOn) //Only run in Traning session
		{
			StartCoroutine(Countdown());
		}

		if (FindObjectsOfType(GetType()).Length == 1)
		{
			DontDestroyOnLoad(gameObject);
			ConfigNewGame();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private IEnumerator Countdown()
	{
		float counter = 900; // 3 seconds you can change this    //20min == 1200 sec   //17min == 1020 sec //15 == 900 //10min == 600  //12min == 720 //13 ==780
							  //to whatever you want
		while (counter > 0)
		{
			yield return new WaitForSecondsRealtime(1);
			

			timeTraningInt = Mathf.RoundToInt(counter);

			float minutes = Mathf.FloorToInt(timeTraningInt / 60);
			float seconds = Mathf.FloorToInt(timeTraningInt % 60);
			timeTraningString = string.Format("{0:00}:{1:00}", minutes, seconds);

			//Debug.Log("Rem time: " + timeTraningString);
			counter--;
		}
		Debug.Log("Times Up ");
		traningTimeOverCanvas.gameObject.SetActive(true);
		Time.timeScale = 0;
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

	public void SaveGameState()
	{ // also called before Mario dies
		LevelManager t_LevelManager = FindObjectOfType<LevelManager>();
		marioSize = t_LevelManager.marioSize;
		deaths = t_LevelManager.deaths;
		coins = t_LevelManager.coins;
		scores = t_LevelManager.scores;
		timeElapsed = t_LevelManager.timeElapsed; // "+ LevelManager.loadSceneDelay" LevelManager.loadSceneDelay is added to add the (extra)time to read feedback!
		hurryUp = t_LevelManager.hurryUp;
		
		

		Invoke("SetBoolBack", 5f);

	}

        private void SetBoolBack()
	{
		dontShowDelayedFeedbackWhenDied = false;
	}


	public void savePerformanceInFile(string context,string diedfrom = "N/A")
	{  //isTraningSession is based on "isnormalGamePlayOn"
		Debug.Log("***Saving Performace Matrics***");
		LevelManager t_LevelManager = FindObjectOfType<LevelManager>();
		//Path of the file
		string path = Application.dataPath + "/PerformaceMetrics.txt";  //Old: Application.dataPath
		
		//Create File if it doesn't exist
		if (!File.Exists(path))
		{
			File.WriteAllText(path, "Performance Metric \n\n");
		}

        string content = "From Traning Session: " + !Constants.isnormalGamePlayOn + "\n" + 
						 "Context: " + context + "\n"+
						 "diedfrom: "+ diedfrom + "\n" +
						 "Level: " + SceneManager.GetActiveScene().name + "\n" +
                         "Score: " + t_LevelManager.scores + "\n" +
                         "Deaths: " + t_LevelManager.deaths + "\n" +
                         "Time: " + t_LevelManager.timeElapsed + "\n\n";
        //Add some to text to it
        File.AppendAllText(path, content);
	}

}
