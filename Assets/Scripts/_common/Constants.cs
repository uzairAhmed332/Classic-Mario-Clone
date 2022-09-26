using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants 
{
    public const bool IS_FEEDBACK_DELAYED = true;

    public const string ENEMY_GOOMBA = "Goomba"; //Brown Goomba,Green Goomba
    public const string ENEMY_KOOPA = "Koopa";
    public const string ENEMY_PLANES = "Planes";

    //1
    public const string FEEDBACK_TITLE_MISSED_COLLECTABLE_BLOCK = "You just missed collectable block";
    public const string FEEDBACK_DESCRIPTION_MISSED_COLLECTABLE_BLOCK = "- Headbutt blocks with question mark \n- You will get valuable things like scores and powerups!";
    public static int   FEEDBACK_MISSED_COLLECTABLE_BLOCK_COUNT = 0;
    //2
    public const string FEEDBACK_TITLE_MARIO_DIED_FROM_ENEMY = "Mario dies as it hit by the enemy"; //U0001F601 //\U0001F60E
    public const string FEEDBACK_DESCRIPTION_MARIO_DIED = "- Either stomp the enemy\n- Or, Jump past an enemy";
    public static int   FEEDBACK_MARIO_DIED_FROM_ENEMY_COUNT = 0;
    //3
    public const string FEEDBACK_TITLE_MARIO_DIED_FROM_PLANE = "Mario dies as it falls off the platform";
    public const string FEEDBACK_DESCRIPTION_PLANE= "- Jump higher by long pressing button 'Z' \n- Jump while running makes jump further";
    public static int   FEEDBACK_MARIO_DIED_FROM_PLANE_COUNT = 0;
    //4
    public const string FEEDBACK_TITLE_LOST_ENEMY = "You lost Score by losing an enemy";
    public const string FEEDBACK_DESCRIPTION_LOST_ENEMY = "kill the enemy before it fell to the ground to gain more score";
    public static int   FEEDBACK_OUT_OF_LOST_ENEMY_COUNT = 0;
    //5
    public const string FEEDBACK_TITLE_OUT_OF_SIGHT_ENEMY = "Enemy went out of sight. You might lose score!";
    public const string FEEDBACK_DESCRIPTION_OUT_OF_SIGHT_ENEMY = "Remember: Mario can only go to the left few steps. Better to kill enemy when its on the right side!";
    public static int   FEEDBACK_OUT_OF_SIGHT_ENEMY_COUNT = 0;
    //6
    public const string FEEDBACK_TITLE_MISSED_BONUS_LEVEL = "You just missed bonus level!";
    public const string FEEDBACK_DESCRIPTION_MISSED_BONUS_LEVEL = "Better to press down whenever Mario is standing on pipe!";
    public static int FEEDBACK_MISSED_BONUS_LEVEL_COUNT = 0;
    //7
    public const string FEEDBACK_TITLE_MISSED_COIN = "You just missed a coin!";
    public const string FEEDBACK_DESCRIPTION_MISSED_COIN = "Catch all the coins to maximize the score";
    public static int FEEDBACK_MISSED_COIN_COUNT = 0;


    //Enemies Names in Inspector
    //Level 1
    public const string Brown_Goomba= "Brown";
    public const string Green_Koopa= "Green";

    // U+1F60C
}

