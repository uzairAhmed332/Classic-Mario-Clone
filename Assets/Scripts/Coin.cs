using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
	private LevelManager t_LevelManager;
    private bool isCoinTaken =false;

    // Use this for initialization
    void Start () {
		t_LevelManager = FindObjectOfType<LevelManager> ();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			isCoinTaken = true;
			t_LevelManager.AddCoin ();
			Destroy (gameObject);
		}
	}

	private void OnBecameInvisible()
	{
		if (!isCoinTaken)
		{
			t_LevelManager.FeedbackActivaotor(Constants.FEEDBACK_TITLE_MISSED_COIN, Constants.FEEDBACK_DESCRIPTION_MISSED_COIN);
			Constants.FEEDBACK_MISSED_COIN_COUNT++;
/*			t_LevelManager.feedbackPanel.gameObject.SetActive(true);
			t_LevelManager.feedbackPanelTitleText.text = Constants.FEEDBACK_TITLE_MISSED_COIN;
			t_LevelManager.feedbackPanelDecsriptionText.text = Constants.FEEDBACK_DESCRIPTION_MISSED_COIN;
			Time.timeScale = 0f;*/
		}
	}
}
