using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillPlane : MonoBehaviour {
	private LevelManager t_LevelManager;
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
		} else {
			//Todo Make feeback when enemy falls to plane and misses by Mario for better score!
			
			Debug.Log(this.name + " ---- " + t_LevelManager.feedbackPanel.transform.GetChild(0).gameObject.name.ToString());
			
			Debug.Log(this.name + " onPlanecollide " + other.gameObject.name);
			
			Destroy (other.gameObject); //hack:Called when Enemy (Non Mario!) dies by hitting planes and ...
			StartCoroutine(showFeedback());
		}
	}

	IEnumerator showFeedback(float delay = 10f)
	{
		t_LevelManager.feedbackPanel.gameObject.SetActive(true);
		Image image = t_LevelManager.feedbackPanel.GetComponent<Image>();
		var tempColor = image.color;
		tempColor.a = 0.4f;
		//image.color = Color.green;
		t_LevelManager.feedbackPanelTitleText.text = "You lost Score by losing an enemy";
		t_LevelManager.feedbackPanelDecsriptionText.text = "kill the enemy before it fell to the down by jumping over it";
		//todo Stop the time!
		mario.Freeze();
		
		yield return new WaitForSeconds(delay);
		t_LevelManager.feedbackPanel.gameObject.SetActive(false);
		mario.UnfreezeUserInput();
	}
}
