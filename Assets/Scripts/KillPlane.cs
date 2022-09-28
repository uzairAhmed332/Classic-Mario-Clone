using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillPlane : MonoBehaviour {
	private LevelManager t_LevelManager;
	public Button feedback_button;
	private Mario mario;
	// Use this for initialization

	void Start () {
		t_LevelManager = FindObjectOfType<LevelManager> ();
		mario = FindObjectOfType<Mario>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {  //Called when mario dies by drawning by hitting planes and ...
			t_LevelManager.MarioRespawn(Constants.ENEMY_PLANES);
		} else if(other.gameObject.tag == "Enemy") {
			//Todo Make feeback when enemy falls to plane and misses by Mario for better score!
			
			Debug.Log(this.name + " ---- " + t_LevelManager.feedbackPanel.transform.GetChild(3).gameObject.name.ToString());
			Debug.Log(this.name + " onPlanecollide " + other.gameObject.name);
			
			Destroy (other.gameObject); //hack:Called when Enemy (Non Mario!) dies by hitting planes and ...
			//StartCoroutine(showFeedback());
			//t_LevelManager.timerPaused = true;
			//t_LevelManager.feedbackPanel.gameObject.SetActive(true);


			t_LevelManager.FeedbackActivaotor(Constants.FEEDBACK_TITLE_LOST_ENEMY, Constants.FEEDBACK_DESCRIPTION_LOST_ENEMY);
			Constants.FEEDBACK_OUT_OF_LOST_ENEMY_COUNT++;
			Debug.Log("xxx"+Constants.FEEDBACK_OUT_OF_LOST_ENEMY_COUNT);


			//Image image = t_LevelManager.feedbackPanel.GetComponent<Image>();
			//var tempColor = image.color;
			//tempColor.a = 0.4f;
			//image.color = Color.green;

			//todo Stop the time!
			//Time.timeScale = 0f;
			//mario.Freeze();
		}
	}

	IEnumerator showFeedback(float delay = 10f)
	{
		t_LevelManager.feedbackPanel.gameObject.SetActive(true);
		Image image = t_LevelManager.feedbackPanel.GetComponent<Image>();
		var tempColor = image.color;
		tempColor.a = 0.4f;
		//image.color = Color.green;
		t_LevelManager.feedbackPanelTitleText.text = Constants.FEEDBACK_TITLE_LOST_ENEMY;
		t_LevelManager.feedbackPanelDecsriptionText.text = Constants.FEEDBACK_DESCRIPTION_LOST_ENEMY;
		//todo Stop the time!
		//mario.Freeze();
		
		yield return new WaitForSeconds(delay);
		t_LevelManager.feedbackPanel.gameObject.SetActive(false);
		mario.UnfreezeUserInput();
	}
}
