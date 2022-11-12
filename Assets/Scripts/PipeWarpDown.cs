﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeWarpDown : MonoBehaviour {
	private LevelManager t_LevelManager;
	private Ghost t_Ghost;
	private Mario mario;
	private Transform stop;
	private bool isMoving;
	private bool isbottomPipeTriggered = false;  //For delayed actual mario

	private float platformVelocityY = -0.05f;
	public string sceneName;
	public bool leadToSameLevel = true;

	public static int marioEnteredCount = 0;

	// Use this for initialization
	void Start () {
		t_LevelManager = FindObjectOfType<LevelManager> ();
		mario = FindObjectOfType<Mario> ();
		stop = transform.parent.transform.Find ("Platform Stop");
		t_Ghost= FindObjectOfType<Ghost>();
	}

	void FixedUpdate() {

			if (isMoving)
			{
				if (transform.position.y > stop.position.y)
				{
					if (!t_LevelManager.timerPaused)
					{
						t_LevelManager.timerPaused = true;
					}
					transform.position = new Vector2(transform.position.x, transform.position.y + platformVelocityY);
				}
				else
				{
					if (leadToSameLevel)
					{
						t_LevelManager.LoadSceneCurrentLevel(sceneName);  //"World 1-1 - Underground"
						this.gameObject.GetComponent<PipeWarpDown>().enabled = false;
					t_Ghost.StopRecordingGhost();
				}
					else
					{
						t_LevelManager.LoadNewLevel(sceneName);
					}
			}
		}
		}

	bool marioEntered = false;
	void OnTriggerStay2D(Collider2D other) {


	

		if (other.tag == "Player" && mario.isCrouching && !marioEntered && marioEnteredCount == 0) {	
			marioEnteredCount++;
			mario.AutomaticCrouch ();
			isMoving = true;
			marioEntered = true;
			t_LevelManager.musicSource.Stop ();
			t_LevelManager.soundSource.PlayOneShot (t_LevelManager.pipePowerdownSound);

		}

		if (other.tag == "Player" && this.name.Equals("Platformxxx") && !marioEntered && marioEnteredCount == 0 && !isbottomPipeTriggered)  //fOR Delayed actual mario becasue he does not croch so previous If condition doesnt work!
		{
			marioEnteredCount++;
			mario.AutomaticCrouch();
			isMoving = true;
			marioEntered = true;
			t_LevelManager.musicSource.Stop();
			t_LevelManager.soundSource.PlayOneShot(t_LevelManager.pipePowerdownSound);
			isbottomPipeTriggered = true;

		}

	}

	private void OnBecameInvisible()
	{
			if(marioEnteredCount == 0)
		{
			t_LevelManager.FeedbackActivaotor(Constants.FEEDBACK_TITLE_MISSED_BONUS_LEVEL, Constants.FEEDBACK_DESCRIPTION_MISSED_BONUS_LEVEL);
			Constants.FEEDBACK_MISSED_BONUS_LEVEL_COUNT++;
		}
	}
		
}
