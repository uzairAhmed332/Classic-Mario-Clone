﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Toad : MonoBehaviour {
	public GameObject ThankYouMario;
	public GameObject ButOurPrincess;

	private Mario mario;
	private LevelManager t_LevelManager;
	private GameStateManager t_GameStateManager;

	// Use this for initialization
	void Start () {
		mario = FindObjectOfType<Mario> ();
		t_LevelManager = FindObjectOfType<LevelManager> ();
		t_GameStateManager = FindObjectOfType<GameStateManager>();
	}


	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			mario.FreezeUserInput ();
			StartCoroutine (DisplayMessageCo ());
		}
	}

	IEnumerator DisplayMessageCo() {
		ThankYouMario.SetActive (true);
		yield return new WaitForSecondsRealtime (.75f);
		ButOurPrincess.SetActive (true);
		yield return new WaitForSecondsRealtime (t_LevelManager.castleCompleteMusic.length);
		t_GameStateManager.savePerformanceInFile(); //filing
		SceneManager.LoadScene ("Main Menu");  //OLD

		//t_LevelManager.LoadNewLevel("Main Menu", t_LevelManager.levelCompleteMusic.length);
	}
}
