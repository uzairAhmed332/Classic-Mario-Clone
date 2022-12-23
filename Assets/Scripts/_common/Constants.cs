using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants 
{
    //Scene Name: "World 1-1","World 1-2",

    public const bool isghostModeOn = true; //Not using anywhere

    //Make either true but not both (Immedate or delayed)
    //For recroding:
    //1. Make true isghostModeImmediateOn & isBeforeghostModeDelayedOn
    //2. Enable t_Ghost.StartRecordingGhost(); from levelManager
    
    public static bool isghostModeImmediateOn = false;
    public static bool isBeforeghostModeDelayedOn = true; //Normal gameplay without ghost. For *SAVING* current movements.
                                                          //If ture then it saves in filing for delayed feedback
                                                          //Sequence:
                                                          //1. For Delayed FB make isBeforeghostModeDelayedOn to "true" and isghostModeDelayedOn to "false" & "isghostModeImmediateOn" to "true"
                                                          //2. When level ends make isBeforeghostModeDelayedOn to "false" and isghostModeDelayedOn to "true" for Replay (Actual mario and ghost)
                                                          //3. When replay ends make isghostModeDelayedOn to "false" & (isBeforeghostModeDelayedOn remains false)
                                                          //4. When both are "false" means go to NEXT level. Before stating next level makes (follow step 1 - 4)

    public static bool isghostModeDelayedOn = false;   // For *LOADING* Actual mario reply & Ghost mario feedback.This will used AFTER level ends

    //isBeforeghostModeDelayedOn becomes false and isghostModeDelayedOn becomes false
    public static bool isnormalGamePlayOn = false;  //NOTE: keep this false unless when playing normal gameplay

    /*
     When making gamplay for immediate feedback
     1. Onyl make "isghostModeImmediateOn" true, make others false

    When making gamplay for deayed feedback
    1. Only make "isBeforeghostModeDelayedOn" true, make others false

    todo: What about normal gameplay
    1. Make "isnormalGamePlayOn" True and allfalse.
     */

    public const string LOAD_LVL1_1_IMMEDAITE_FEEDBACK_VIDEO = "/GhostLvl1_1"; //From start till bonus level  //Done
    public const string LOAD_LVL1_2_IMMEDAITE_FEEDBACK_VIDEO = "/GhostLvl1_2"; //Only Bonus level //Done
    public const string LOAD_LVL1_3_IMMEDAITE_FEEDBACK_VIDEO = "/GhostLvl1_3"; //After bonus level till end //Done

    public const string LOAD_LVL1_1_Delayed_FEEDBACK_VIDEO = "/GhostLvl1_1d"; //From start till bonus level  
    public const string LOAD_LVL1_2_Delayed_FEEDBACK_VIDEO = "/GhostLvl1_2d"; //Only Bonus level 
    public const string LOAD_LVL1_3_Delayed_FEEDBACK_VIDEO = "/GhostLvl1_3d"; //After bonus level till end 

     
    public const string LOAD_LVL2_1_IMMEDAITE_FEEDBACK_VIDEO = "/GhostLvl2_1"; //From start till bonus level //Done
    public const string LOAD_LVL2_2_IMMEDAITE_FEEDBACK_VIDEO = "/GhostLvl2_2"; //Only Bonus level //Done
    public const string LOAD_LVL2_3_IMMEDAITE_FEEDBACK_VIDEO = "/GhostLvl2_3"; //After bonus level till end //Done

    public const string LOAD_LVL2_1_Delayed_FEEDBACK_VIDEO = "/GhostLvl2_1d"; //From start till bonus level  
    public const string LOAD_LVL2_2_Delayed_FEEDBACK_VIDEO = "/GhostLvl2_2d"; //Only Bonus level 
    public const string LOAD_LVL2_3_Delayed_FEEDBACK_VIDEO = "/GhostLvl2_3d"; //After bonus level till end 



    public const bool IS_FEEDBACK_DELAYED = false;  //False == immediate  feedback, true == Delayed feedback
    public const bool NO_FEEDBACK = false; //When true -->Run like delayed but last screen like immediate

    public const string ENEMY_GOOMBA = "Goomba"; //Brown Goomba,Green Goomba
    public const string ENEMY_KOOPA = "Koopa";
    public const string ENEMY_PIRANHA = "Piranha";
    
    public const string ENEMY_PLANES = "Planes";

    public const string FIRE_BAR = "Firebar";
    public const string BOWSER_FIRE = "Bowser fire";
    public const string BOWSER = "Bowser";

    //1
    public const string FEEDBACK_TITLE_MISSED_COLLECTABLE_BLOCK = "Missed a collectable block";//"You just missed a collectable block";
    public const string FEEDBACK_DESCRIPTION_MISSED_COLLECTABLE_BLOCK = "- Headbutt blocks with question marks \n- You will get valuable things like score and power-ups!";
    public static int   FEEDBACK_MISSED_COLLECTABLE_BLOCK_COUNT = 0;
    //2
    public const string FEEDBACK_TITLE_MARIO_DIED_FROM_ENEMY = "Mario died";//"Mario dies if hit by an enemy"; //U0001F601 //\U0001F60E
    public const string FEEDBACK_DESCRIPTION_MARIO_DIED = "- Either stomp the enemy\n- Or, Jump past the enemy";
    public static int   FEEDBACK_MARIO_DIED_FROM_ENEMY_COUNT = 0;
    //3
    public const string FEEDBACK_TITLE_MARIO_DIED_FROM_PLANE = "Mario died";//"Mario dies if he falls into a pit";
    public const string FEEDBACK_DESCRIPTION_PLANE= "- Jump higher by pressing the a-button 'Z' for longer \n- Jump while running to jump further";
    public static int   FEEDBACK_MARIO_DIED_FROM_PLANE_COUNT = 0;
    //4
    public const string FEEDBACK_TITLE_LOST_ENEMY = "Enemy missed";//"You just lost Score by missing an enemy";
    public const string FEEDBACK_DESCRIPTION_LOST_ENEMY = "Defeat the enemy before it disappears to gain more score";
    public static int   FEEDBACK_OUT_OF_LOST_ENEMY_COUNT = 0;
    //5
    public const string FEEDBACK_TITLE_OUT_OF_SIGHT_ENEMY = "";//"The enemy went out of sight. You might lose score!";
    public const string FEEDBACK_DESCRIPTION_OUT_OF_SIGHT_ENEMY = "Remember: Mario can only go to the left a few steps. Try to defeat enemies while they are still in sight!";
    public static int   FEEDBACK_OUT_OF_SIGHT_ENEMY_COUNT = 0;
    //6
    public const string FEEDBACK_TITLE_MISSED_BONUS_LEVEL = "Missed Bonus level";//"You have missed score from a bonus level!";
    public const string FEEDBACK_DESCRIPTION_MISSED_BONUS_LEVEL = "Try to press down whenever Mario is standing on a pipe!";
    public static int FEEDBACK_MISSED_BONUS_LEVEL_COUNT = 0;
    //7
    public const string FEEDBACK_TITLE_MISSED_COIN =  "Missed a coin";//"You just missed a coin and lost score!";
    public const string FEEDBACK_DESCRIPTION_MISSED_COIN = "Catch all the coins to maximize the score";
    public static int FEEDBACK_MISSED_COIN_COUNT = 0;


    //Enemies Names in Inspector
    //Level 1
    public const string Brown_Goomba= "Brown";
    public const string Green_Koopa= "Green";

    public const string SAVED_WHEN_LEVEL_END = "Saved when level ends";
    public const string SAVED_WHEN_MARIO_DIED = "Saved when mario died";
}

