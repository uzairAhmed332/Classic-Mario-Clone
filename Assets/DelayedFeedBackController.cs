using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DelayedFeedBackController : MonoBehaviour
{

   

    public int typeOfFeedBack;
    public TextMeshProUGUI title;

    // Start is called before the first frame update
    void Start()
    {
        //Disable everyfeedback //enable it based on their constant > 0
        //1
        if (Constants.FEEDBACK_MISSED_COLLECTABLE_BLOCK_COUNT > 0) {
            this.gameObject.SetActive(true);
            title.text = "You missed <b>" +  Constants.FEEDBACK_MISSED_COLLECTABLE_BLOCK_COUNT  + "</b> collectable block";
        }
        //2
        if (Constants.FEEDBACK_MARIO_DIED_FROM_ENEMY_COUNT > 0)
        {
            this.gameObject.SetActive(true);
            title.text = "Mario dies <b>" + Constants.FEEDBACK_MARIO_DIED_FROM_ENEMY_COUNT+ "</b> times as it hit by the enemy";
        }
        //3
        if (Constants.FEEDBACK_MARIO_DIED_FROM_PLANE_COUNT > 0)
        {
            this.gameObject.SetActive(true);
            title.text = "Mario dies <b>" + Constants.FEEDBACK_MARIO_DIED_FROM_PLANE_COUNT + "</b> times as it falls off the platform";
        }
        //4
        if (Constants.FEEDBACK_OUT_OF_LOST_ENEMY_COUNT > 0)
        {
            this.gameObject.SetActive(true);
            title.text = "You lost score by losing enemy <b>" + Constants.FEEDBACK_OUT_OF_LOST_ENEMY_COUNT + "</b> times";
        }
        //5
        if (Constants.FEEDBACK_OUT_OF_SIGHT_ENEMY_COUNT > 0)
        {
            this.gameObject.SetActive(true);
            title.text = "Enemy went out of sight <b>" + Constants.FEEDBACK_OUT_OF_SIGHT_ENEMY_COUNT + "</b> times";
        }
        //6
        if (Constants.FEEDBACK_MISSED_BONUS_LEVEL_COUNT > 0)
        {
            this.gameObject.SetActive(true);
            title.text = "You missed a bonus level! <b>" + Constants.FEEDBACK_MISSED_BONUS_LEVEL_COUNT + "</b> times";
        }
        //7
        if (Constants.FEEDBACK_MISSED_COIN_COUNT > 0)
        {
            this.gameObject.SetActive(true);
            title.text = "You lost score by misseg <b>" + Constants.FEEDBACK_MISSED_BONUS_LEVEL_COUNT + "</b> coins ";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
