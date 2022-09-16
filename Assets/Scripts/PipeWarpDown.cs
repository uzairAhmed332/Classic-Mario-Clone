using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeWarpDown : MonoBehaviour {
	private LevelManager t_LevelManager;
	private Mario mario;
	private Transform stop;
	private bool isMoving;

	private float platformVelocityY = -0.05f;
	public string sceneName;
	public bool leadToSameLevel = true;

	// Use this for initialization
	void Start () {
		t_LevelManager = FindObjectOfType<LevelManager> ();
		mario = FindObjectOfType<Mario> ();
		stop = transform.parent.transform.Find ("Platform Stop");
	}

	void FixedUpdate() {
		if (isMoving) {
			if (transform.position.y > stop.position.y) {
				if (!t_LevelManager.timerPaused) {
					t_LevelManager.timerPaused = true;
				}
				transform.position = new Vector2 (transform.position.x, transform.position.y + platformVelocityY);
			} else {
				if (leadToSameLevel) {
					t_LevelManager.LoadSceneCurrentLevel (sceneName);
				} else {
					t_LevelManager.LoadNewLevel (sceneName);
				}
			}
		}
	}

	bool marioEntered = false;
	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Player" && mario.isCrouching && !marioEntered) {
			mario.AutomaticCrouch ();
			isMoving = true;
			marioEntered = true;
			t_LevelManager.musicSource.Stop ();
			t_LevelManager.soundSource.PlayOneShot (t_LevelManager.pipePowerdownSound);
		}
	}

	private void OnBecameInvisible()
	{
				t_LevelManager.feedbackPanel.gameObject.SetActive(true);
				t_LevelManager.feedbackPanelTitleText.text = Constants.FEEDBACK_TITLE_OUT_OF_SIGHT_PIPE;
				t_LevelManager.feedbackPanelDecsriptionText.text = Constants.FEEDBACK_DESCRIPTION_OUT_OF_SIGHT_PIPE;
				Time.timeScale = 0f;
	}
		
}
