﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using TMPro;
using System;

public class LevelStartScreen : MonoBehaviour {
	
	private float loadScreenDelay = 0.0f;

	public Canvas canvas;

	public Text WorldTextHUD;
	public Text ScoreTextHUD;
	public Text CoinTextHUD;
	public Text WorldTextMain;
	public Text livesText;

	public GameObject FB_1;
	public GameObject FB_2;
	public GameObject FB_3;
	public GameObject FB_4;
	public GameObject FB_5;
	public GameObject FB_6;
	public GameObject FB_7;

	public TextMeshProUGUI FB_1_title;
	public TextMeshProUGUI FB_2_title;
	public TextMeshProUGUI FB_3_title;
	public TextMeshProUGUI FB_4_title;
	public TextMeshProUGUI FB_5_title;
	public TextMeshProUGUI FB_6_title;
	public TextMeshProUGUI FB_7_title;

	public TextMeshProUGUI FB_1_desc;
	public TextMeshProUGUI FB_2_desc;
	public TextMeshProUGUI FB_3_desc;
	public TextMeshProUGUI FB_4_desc;
	public TextMeshProUGUI FB_5_desc;
	public TextMeshProUGUI FB_6_desc;
	public TextMeshProUGUI FB_7_desc;

	public Button clickToContinue;

	private LevelManager t_LevelManager;
	private GameStateManager t_GameStateManager;
	// Use this for initialization
	void Start () {

		t_GameStateManager = FindObjectOfType<GameStateManager>();
		t_LevelManager = GetComponent<LevelManager>();
		//Debug.Log("+++ " + t_LevelManager);
		//feedbackConditions(); //just for temp for testing
		if (Constants.IS_FEEDBACK_DELAYED && !t_GameStateManager.dontShowDelayedFeedbackWhenDied && !Constants.NO_FEEDBACK)  
		{
			//Delayed feedback is enabled. 
			Debug.Log(this.name + "1:" + transform.GetChild(0).transform.GetChild(1).gameObject.name); //DelayedFeedbackPanel

			//NOTE: enable this line when OLD way of delayed feedback is required, otherwise it will stay inactive!
			//transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true); // enabling DelayedFeedbackPanel
																			
			Time.timeScale = 1;
			Debug.Log("=== " + t_GameStateManager.sceneToLoad + " | " + loadScreenDelay);
		//	feedbackConditions();

		}
		else {
			transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);  //// disabling DelayedFeedbackPanel
			Time.timeScale = 1;

			string worldName = t_GameStateManager.sceneToLoad;

			WorldTextHUD.text = Regex.Split(worldName, "World ")[1];
			ScoreTextHUD.text = t_GameStateManager.scores.ToString("D6");
			CoinTextHUD.text = "x" + t_GameStateManager.coins.ToString("D2");
			WorldTextMain.text = worldName.ToUpper();
			livesText.text = t_GameStateManager.deaths.ToString();

			start_Corotine();
		}
		clickToContinue.onClick.AddListener(start_Corotine);
	}

	private void start_Corotine()
	{
		if (Constants.IS_FEEDBACK_DELAYED) { 
		transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false); }

		StartCoroutine(LoadSceneDelayCo(t_GameStateManager.sceneToLoad, loadScreenDelay));
		Debug.Log(this.name + " Start: current scene is " + SceneManager.GetActiveScene().name);
	}

    private void feedbackConditions()
    {
		//Disable everyfeedback //enable it based on their constant > 0
		//1
		if (Constants.FEEDBACK_MISSED_COLLECTABLE_BLOCK_COUNT > 0)
		{
			FB_1.gameObject.SetActive(true);
			FB_1_title.text = "You missed collectable blocks <b><color=red>" + Constants.FEEDBACK_MISSED_COLLECTABLE_BLOCK_COUNT + "</b></color> times";
			FB_1_desc.text = "- Headbutt blocks with question marks \n- You will get valuable things like score and power-ups!";
		}
		//2
		if (Constants.FEEDBACK_MARIO_DIED_FROM_ENEMY_COUNT > 0)
		{
			FB_2.gameObject.SetActive(true);
			FB_2_title.text = "Mario dies <b><color=red>" + Constants.FEEDBACK_MARIO_DIED_FROM_ENEMY_COUNT + "</b></color> times because he was hit by the enemy";
			FB_2_desc.text = "- Either stomp the enemy\n- Or, Jump past the enemy";
		}
		//3
		if (Constants.FEEDBACK_MARIO_DIED_FROM_PLANE_COUNT > 0)
		{
			FB_3.gameObject.SetActive(true);
			FB_3_title.text = "Mario died <b><color=red>" + Constants.FEEDBACK_MARIO_DIED_FROM_PLANE_COUNT + "</b></color> times because he fell into a pit";
			FB_3_desc.text = "- Jump higher by pressing the a-button 'Z' for longer\n- Jump while running to jump further";
		}
		//4
		if (Constants.FEEDBACK_OUT_OF_LOST_ENEMY_COUNT > 0)
		{
			FB_4.gameObject.SetActive(true);
			FB_4_title.text = "You lost score by missing enemies <b><color=red>" + Constants.FEEDBACK_OUT_OF_LOST_ENEMY_COUNT + "</b></color> times";
			FB_4_desc.text = "Defeat the enemies before they disappear to gain more score";
		}
		//5
		if (Constants.FEEDBACK_OUT_OF_SIGHT_ENEMY_COUNT > 0)
		{
			FB_5.gameObject.SetActive(true);
			FB_5_title.text = "The enemy went out of sight <b><color=red>" + Constants.FEEDBACK_OUT_OF_SIGHT_ENEMY_COUNT + "</b></color> times. You could've lost score!";
			FB_5_desc.text = "Remember: Mario can only go to the left a few steps. Try to defeat enemies while they are still in sight!";
		}
		//6
		if (Constants.FEEDBACK_MISSED_BONUS_LEVEL_COUNT > 0)
		{
			FB_6.gameObject.SetActive(true);
			FB_6_title.text = "You have missed score from a bonus level!";
			FB_6_desc.text = "Try to press down whenever Mario is standing on a pipe!";
		}
		//7
		if (Constants.FEEDBACK_MISSED_COIN_COUNT > 0)
		{
			FB_7.gameObject.SetActive(true);
			FB_7_title.text = "You lost score by missing <b><color=red>" + Constants.FEEDBACK_MISSED_COIN_COUNT + "</b></color> coins";
			FB_7_desc.text = "Catch all coins to maximize the score";
		}
	}

    private void Update()
    {
	

	}

    IEnumerator LoadSceneDelayCo(string sceneName, float delay) {
		yield return new WaitForSecondsRealtime (delay);
		SceneManager.LoadScene (sceneName);
	}
}
