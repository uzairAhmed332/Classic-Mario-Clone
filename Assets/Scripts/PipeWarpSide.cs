using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PipeWarpSide : MonoBehaviour {
	private LevelManager t_LevelManager;
	private Ghost t_Ghost;
	private Mario mario;
	private bool reachedPortal;

	public string sceneName;
	public int spawnPipeIdx;
	public bool leadToSameLevel = true;
	private bool sec_delay_10 = true;
	// Use this for initialization
	void Start () {
		t_LevelManager = FindObjectOfType<LevelManager> ();
		mario = FindObjectOfType<Mario> ();
		t_Ghost = FindObjectOfType<Ghost>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			mario.AutomaticWalk (mario.levelEntryWalkSpeedX);
			reachedPortal = true;
			t_LevelManager.timerPaused = true;
			Debug.Log (this.name + " OnTriggerEnter2D: " + transform.parent.gameObject.name 
				+ " recognizes player, should automatic walk");
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player" && reachedPortal) {
			t_LevelManager.soundSource.PlayOneShot (t_LevelManager.pipePowerdownSound);

			if (leadToSameLevel) {
				Debug.Log (this.name + " OnCollisionEnter2D: " + transform.parent.gameObject.name 
					+ " teleports player to different scene same level " + sceneName
					+ ", pipe idx " + spawnPipeIdx);
				t_LevelManager.LoadSceneCurrentLevelSetSpawnPipe (sceneName, spawnPipeIdx);

				//if (sec_delay_10)
			//	{
				//	sec_delay_10 = false;
					LevelManager.comingFromPipe = true;  //Set booloan value to "true" here for ghost video after coming out from pipe (Bonus level) //False it after 10 sec
					//Invoke("SetBoolBack", 10f);                          
				//}			

				if (Constants.isBeforeghostModeDelayedOn)
				{
					t_Ghost.StopRecordingGhost();
				}
			} else {
				Debug.Log (this.name + " OnCollisionEnter2D: " + transform.parent.gameObject.name
					+ " teleports player to new level " + sceneName 
					+ ", pipe idx " + spawnPipeIdx);
				t_LevelManager.LoadNewLevel (sceneName);  //I think this will never called! if it does then check conditons of LoadNewLevel for feedback!
			}
		}
	}

/*	private void SetBoolBack()
	{
		sec_delay_10 = true;
		LevelManager.comingFromPipe = false;
	}*/

}
