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
		if (other.gameObject.tag == "Player") {  //Called when mario dies by drawning by hitting planes and ...
			t_LevelManager.MarioRespawn(Constants.ENEMY_PLANES);
		} else {
			//Todo Make feeback when enemy falls to plane and misses by Mario for better score!
			t_LevelManager.feedbackPanelWhenItemMissed.gameObject.SetActive(true);
			
			Debug.Log(this.name + " ---- " + t_LevelManager.feedbackPanel.transform.GetChild(0).gameObject.name.ToString());
			Debug.Log(this.name + " onPlanecollide " + other.gameObject.name);
			
			Destroy (other.gameObject); //hack:Called when Enemy (Non Mario!) dies by hitting planes and ...
			StartCoroutine(showFeedback());
		}
	}

	IEnumerator showFeedback(float delay = 5f)
	{
		yield return new WaitForSeconds(delay);
		t_LevelManager.feedbackPanelWhenItemMissed.gameObject.SetActive(false);
	}
}
