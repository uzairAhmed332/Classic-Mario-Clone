using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Move continuously, flipping direction if hit on the side by non-Player. Optionally
 * bounce up if hit ground while moving.
 * Applicable to: 1UP Mushroom, Big Mushroom, Starman, Goomba, Koopa
 */

public class MoveAndFlip : MonoBehaviour {
	static int counterOutOfSightFeedback = 0;
	public bool canMove = false;
	public bool canMoveAutomatic = true;
	private float minDistanceToMove = 14f;

	public float directionX = 1;
	public Vector2 Speed = new Vector2 (3, 0);
	private Rigidbody2D m_Rigidbody2D;
	private GameObject mario;
	private Mario t_mario;
	private Enemy enemy;
	private LevelManager t_LevelManager;
	Coroutine routine;

	bool isFeedbackRunning = false;

	public Transform t_transform;
	int count_pos_y = 0;
	bool doNotShowOutOfSightFeedback = false;
	void Start () {
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		t_LevelManager = FindObjectOfType<LevelManager>();
		enemy = GetComponent<Enemy>();
		mario = FindObjectOfType<Mario> ().gameObject;
		t_mario = FindObjectOfType<Mario>();
		//enemy_pos_y= t_transform.position.y;
		OrientSprite ();
	}


	void Update() {
        //Debug.Log(this.name + "Pos y => " + (transform.position.y));
        if (transform.position.y <= -1 )
        {
           // count_pos_y++;
            doNotShowOutOfSightFeedback = true;
        }
       /* else
        {
            showEnemyDiedFeedback = false;
        }*/
        if (!canMove & Mathf.Abs (mario.transform.position.x - transform.position.x) <= minDistanceToMove && canMoveAutomatic) {
			canMove = true; //comment for temp purpose
			Debug.Log(this.name + " Enemy now moving!");
		}
	}


	void FixedUpdate()
	{
		if (canMove)
		{
			m_Rigidbody2D.velocity = new Vector2(Speed.x * directionX, m_Rigidbody2D.velocity.y);
		}
	}

	/* private void OnBecameVisible()
	 {
		 Time.timeScale = 0f;
	 }*/

	private void OnBecameInvisible()
	{
		if(!enemy.isBeingStomped){
		if (this.name.Contains(Constants.Brown_Goomba)
			&& counterOutOfSightFeedback <2
			&& !doNotShowOutOfSightFeedback)
		{
			doNotShowOutOfSightFeedback = true; //Now this will always be true and shows ememy died feedback
			counterOutOfSightFeedback++;
			//isFeedbackRunning = true;
			t_LevelManager.feedbackPanel.gameObject.SetActive(true);
			t_LevelManager.feedbackPanelTitleText.text = Constants.FEEDBACK_TITLE_OUT_OF_SIGHT_ENEMY;
			t_LevelManager.feedbackPanelDecsriptionText.text = Constants.FEEDBACK_DESCRIPTION_OUT_OF_SIGHT_ENEMY;
			Time.timeScale = 0f;
			//canMove = false;
			}
		}
	}
    IEnumerator showFeedback(string enemyName,float delay = 10f)
	{
		counterOutOfSightFeedback++;

		  //t_mario.Freeze();
		isFeedbackRunning = true;
		t_LevelManager.feedbackPanel.gameObject.SetActive(true);
		t_LevelManager.feedbackPanelTitleText.text = Constants.FEEDBACK_TITLE_OUT_OF_SIGHT_ENEMY;
		t_LevelManager.feedbackPanelDecsriptionText.text = Constants.FEEDBACK_DESCRIPTION_OUT_OF_SIGHT_ENEMY;
		yield return new WaitForSeconds(delay);
		t_LevelManager.feedbackPanel.gameObject.SetActive(false);
		isFeedbackRunning = false;
		StopCoroutine(routine);
		//t_mario.UnfreezeUserInput();

	}


	//	void OnBecameVisible() {
	//		if (canMoveAutomatic) {
	//			canMove = true;
	//		}
	//	}

	// Assuming default sprites face right
	void OrientSprite() {
		if (directionX > 0) {
			transform.localScale = new Vector3 (1, 1, 1);
		} else if (directionX < 0) {
			transform.localScale = new Vector3 (-1, 1, 1);
		}
	}


	void OnCollisionEnter2D(Collision2D other) {
		Vector2 normal = other.contacts[0].normal;
		Vector2 leftSide = new Vector2 (-1f, 0f);
		Vector2 rightSide = new Vector2 (1f, 0f);
		Vector2 bottomSide = new Vector2 (0f, 1f);
		bool sideHit = normal == leftSide || normal == rightSide;
		bool bottomHit = normal == bottomSide;

		// reverse direction
		if (other.gameObject.tag != "Player" && sideHit) {
			directionX = -directionX;
			OrientSprite ();
		}

		else if (other.gameObject.tag.Contains("Platform") && bottomHit && canMove) {
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Speed.y);
		}
	}


}

