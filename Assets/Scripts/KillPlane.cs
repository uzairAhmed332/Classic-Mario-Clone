using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour {
	private LevelManager t_LevelManager;

	// Use this for initialization
	void Start () {
		t_LevelManager = FindObjectOfType<LevelManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {  //Called when mario dies by drawning like planes and ...
			t_LevelManager.MarioRespawn(Constants.ENEMY_PLANES);
		} else {
			Destroy (other.gameObject);
		}
	}
}
