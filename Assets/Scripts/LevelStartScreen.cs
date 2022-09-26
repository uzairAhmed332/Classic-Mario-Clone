using System.Collections;
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
		
		if (Constants.IS_FEEDBACK_DELAYED && !t_GameStateManager.delayWhenGamestatesaved)
		{
			//Delayed feedback is enabled. 
			Debug.Log(this.name + "1:" + transform.GetChild(0).transform.GetChild(1).gameObject.name); //DelayedFeedbackPanel
			transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true); // enabling DelayedFeedbackPanel
																					//Debug.Log(this.name + "2: " + transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0)); //Content //On this
																					//transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);

			//if (Constants.FEEDBACK_MISSED_COLLECTABLE_BLOCK_COUNT>0) {
			//	title.text = "You missed <b>" + Constants.FEEDBACK_MISSED_COLLECTABLE_BLOCK_COUNT + "</b> collectable block";
			//}
			Time.timeScale = 1;
			Debug.Log("=== " + t_GameStateManager.sceneToLoad + " | " + loadScreenDelay);
			feedbackConditions();

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
		if (Constants.FEEDBACK_MISSED_COLLECTABLE_BLOCK_COUNT > 0)
		{
			FB_1.gameObject.SetActive(true);
			FB_1_title.text = "You missed <b>" + Constants.FEEDBACK_MISSED_COLLECTABLE_BLOCK_COUNT + "</b> collectable block";
			FB_1_desc.text = "- Headbutt blocks with question mark \n- You will get valuable things like scores and powerups!";
		}
		//2
		if (Constants.FEEDBACK_MARIO_DIED_FROM_ENEMY_COUNT > 0)
		{
			FB_2.gameObject.SetActive(true);
			FB_2_title.text = "Mario dies <b>" + Constants.FEEDBACK_MARIO_DIED_FROM_ENEMY_COUNT + "</b> times as it hit by the enemy";
			FB_2_desc.text = "- Either stomp the enemy\n- Or, Jump past an enemy";
		}
		//3
		if (Constants.FEEDBACK_MARIO_DIED_FROM_PLANE_COUNT > 0)
		{
			FB_3.gameObject.SetActive(true);
			FB_3_title.text = "Mario dies <b>" + Constants.FEEDBACK_MARIO_DIED_FROM_PLANE_COUNT + "</b> times as it falls off the platform";
			FB_3_desc.text = "- Jump higher by long pressing button 'Z' \n- Jump while running makes jump further";
		}
		//4
		if (Constants.FEEDBACK_OUT_OF_LOST_ENEMY_COUNT > 0)
		{
			FB_4.gameObject.SetActive(true);
			FB_4_title.text = "You lost score by losing enemy <b>" + Constants.FEEDBACK_OUT_OF_LOST_ENEMY_COUNT + "</b> times";
			FB_4_desc.text = "kill the enemy before it fell to the ground to gain more score";
		}
		//5
		if (Constants.FEEDBACK_OUT_OF_SIGHT_ENEMY_COUNT > 0)
		{
			FB_5.gameObject.SetActive(true);
			FB_5_title.text = "Enemy went out of sight <b>" + Constants.FEEDBACK_OUT_OF_SIGHT_ENEMY_COUNT + "</b> times";
			FB_5_desc.text = "Remember: Mario can only go to the left few steps. Better to kill enemy when its on the right side!";
		}
		//6
		if (Constants.FEEDBACK_MISSED_BONUS_LEVEL_COUNT > 0)
		{
			FB_6.gameObject.SetActive(true);
			FB_6_title.text = "You missed a bonus level!";
			FB_6_desc.text = "Better to press down whenever Mario is standing on pipe!";
		}
		//7
		if (Constants.FEEDBACK_MISSED_COIN_COUNT > 0)
		{
			FB_7.gameObject.SetActive(true);
			FB_7_title.text = "You lost score by misseg <b>" + Constants.FEEDBACK_MISSED_COIN_COUNT + "</b> coins ";
			FB_7_desc.text = "Catch all the coins to maximize the score";
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
