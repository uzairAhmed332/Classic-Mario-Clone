using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;


public class LevelStartScreen : MonoBehaviour {
	private GameStateManager t_GameStateManager;
	private float loadScreenDelay = 2;

	public Canvas canvas;
	public Text WorldTextHUD;
	public Text ScoreTextHUD;
	public Text CoinTextHUD;
	public Text WorldTextMain;
	public Text livesText;
	public Text testFeedbackText;
	// Use this for initialization
	void Start () {
		//If we delayed feedback enable canvas otherwise keep it disabled
		if (!Constants.IS_FEEDBACK_DELAYED)
		{
			//canvas.gameObject.SetActive(false);
			Time.timeScale = 1;
			t_GameStateManager = FindObjectOfType<GameStateManager>();
			string worldName = t_GameStateManager.sceneToLoad;

			WorldTextHUD.text = Regex.Split(worldName, "World ")[1];
			ScoreTextHUD.text = t_GameStateManager.scores.ToString("D6");
			CoinTextHUD.text = "x" + t_GameStateManager.coins.ToString("D2");
			WorldTextMain.text = worldName.ToUpper();
			livesText.text = t_GameStateManager.deaths.ToString();
			testFeedbackText.text = "xxx " + Constants.FEEDBACK_OUT_OF_SIGHT_ENEMY_COUNT.ToString();

			StartCoroutine(LoadSceneDelayCo(t_GameStateManager.sceneToLoad, loadScreenDelay));

			Debug.Log(this.name + " Start: current scene is " + SceneManager.GetActiveScene().name);
		}
		else { 
		//Delayed feedback is enabled. Make button visible and click on to go to next next level instead of timer

		}
	}
	
	IEnumerator LoadSceneDelayCo(string sceneName, float delay) {
		yield return new WaitForSecondsRealtime (delay);
		SceneManager.LoadScene (sceneName);
	}
}
