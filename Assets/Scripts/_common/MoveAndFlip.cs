using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Move continuously, flipping direction if hit on the side by non-Player. Optionally
 * bounce up if hit ground while moving.
 * Applicable to: 1UP Mushroom, Big Mushroom, Starman, Goomba, Koopa
 */

public class MoveAndFlip : MonoBehaviour {
	public bool canMove = false;
	public bool canMoveAutomatic = true;
	private float minDistanceToMove = 14f;

	public float directionX = 1;
	public Vector2 Speed = new Vector2 (3, 0);
	private Rigidbody2D m_Rigidbody2D;
	private GameObject mario;
	private Mario t_mario;
	private LevelManager t_LevelManager;
	Coroutine routine;

	bool isFeedbackRunning = false;
	// Use this for initialization
	void Start () {
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		t_LevelManager = FindObjectOfType<LevelManager>();
		mario = FindObjectOfType<Mario> ().gameObject;
		t_mario = FindObjectOfType<Mario>();
		OrientSprite ();
	}


	void Update() {
		
		if (!canMove & Mathf.Abs (mario.transform.position.x - transform.position.x) <= minDistanceToMove && canMoveAutomatic) {
			canMove = true; //comment for temp purpose
			if (Mathf.Abs(mario.transform.position.x - (-(transform.position.x))) <= minDistanceToMove) {
				Debug.Log(this.name + " Enemy past!");
			}
			Debug.Log(this.name + " Enemy now moving!");
		}
	}


    private void OnBecameInvisible()
    {
		if(this.name == Constants.Brown_Goomba_1 && !isFeedbackRunning)
        {
			routine= StartCoroutine(showFeedback(this.name));
			Debug.Log(this.name + " Hi");
		
		}

		Debug.Log(this.name + " invisible!");
	}

    IEnumerator showFeedback(string enemyName,float delay = 10f)
	{
		
		//t_mario.Freeze();
		isFeedbackRunning = true;
		t_LevelManager.feedbackPanel.gameObject.SetActive(true);
		t_LevelManager.feedbackPanelTitleText.text = "Enemy went out of sight. You might lose score!";
		t_LevelManager.feedbackPanelDecsriptionText.text = "Mario can only go to the left few steps. Better to kill enemy when its on the right side!";
		yield return new WaitForSeconds(delay);
		t_LevelManager.feedbackPanel.gameObject.SetActive(false);
		isFeedbackRunning = false;
		StopCoroutine(routine);
		StopAllCoroutines();
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

	void FixedUpdate () {
		if (canMove) {
			m_Rigidbody2D.velocity = new Vector2(Speed.x * directionX, m_Rigidbody2D.velocity.y);
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

