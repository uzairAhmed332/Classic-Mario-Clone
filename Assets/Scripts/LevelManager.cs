using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using TMPro;
using System;

public class LevelManager : MonoBehaviour
{
    public const float loadSceneDelay = 1f;

    public bool hurryUp; // within last 100 secs?
    public int marioSize; // 0..2
    public int deaths;
    public int coins;
    public int scores;
    public float timeElapsed;
    private int timeElapsedInt;

    public int coinBonus = 200;
    public int powerupBonus = 1000;
    public int starmanBonus = 1000;
    public int oneupBonus = 0;
    public int breakBlockBonus = 50; //Why its always 50 even when value chnaged? Ans: Value getting from unity editor and its getting prority over code!

    public bool isRespawning;
    private bool isPoweringDown;
    //public bool isFeedbackPanelVisible = false;

    public bool isInvinciblePowerdown;
    public bool isInvincibleStarman;
    private float MarioInvinciblePowerdownDuration = 2;
    private float MarioInvincibleStarmanDuration = 12;
    private float transformDuration = 1;

    private GameStateManager t_GameStateManager;
    private Mario mario;
    private PipeWarpDown pipeWarpDown;

    private Animator mario_Animator;
    private Rigidbody2D mario_Rigidbody2D;

    public Text scoreText;
    public Text coinText;
    public Text timeText;
    public Text deathText;
    public GameObject FloatingTextEffect;
    private const float floatingTextOffsetY = 2f;

 
    public GameObject feedbackPanel;
    public TextMeshProUGUI feedbackPanelTitleText;
    public TextMeshProUGUI feedbackPanelDecsriptionText;

    public AudioSource musicSource;
    public AudioSource soundSource;
    public AudioSource pauseSoundSource;

    public AudioClip levelMusic;
    public AudioClip levelMusicHurry;
    public AudioClip starmanMusic;
    public AudioClip starmanMusicHurry;
    public AudioClip levelCompleteMusic;
    public AudioClip castleCompleteMusic;

    public AudioClip oneUpSound;
    public AudioClip bowserFallSound;
    public AudioClip bowserFireSound;
    public AudioClip breakBlockSound;
    public AudioClip bumpSound;
    public AudioClip coinSound;
    public AudioClip deadSound;
    public AudioClip fireballSound;
    public AudioClip flagpoleSound;
    public AudioClip jumpSmallSound;
    public AudioClip jumpSuperSound;
    public AudioClip kickSound;
    public AudioClip pipePowerdownSound;
    public AudioClip powerupSound;
    public AudioClip powerupAppearSound;
    public AudioClip stompSound;
    public AudioClip warningSound;



    public Vector2 stompBounceVelocity = new Vector2(0, 15);

    public bool gamePaused;
    public bool timerPaused;
    public bool musicPaused;


    void Awake()
    {
        Time.timeScale = 1;
    }

    // Use this for initialization
    void Start()
    {
        t_GameStateManager = FindObjectOfType<GameStateManager>();
        RetrieveGameState();

        mario = FindObjectOfType<Mario>();
        pipeWarpDown = FindObjectOfType<PipeWarpDown>();
        

        mario_Animator = mario.gameObject.GetComponent<Animator>();
        mario_Rigidbody2D = mario.gameObject.GetComponent<Rigidbody2D>();
        mario.UpdateSize();

        // Sound volume
        musicSource.volume = PlayerPrefs.GetFloat("musicVolume");
        soundSource.volume = PlayerPrefs.GetFloat("soundVolume");
        pauseSoundSource.volume = PlayerPrefs.GetFloat("soundVolume");

        // HUD
        SetHudCoin();
        SetHudScore();
        SetHudDeath();
        SetHudTime();
        if (hurryUp)
        {
            ChangeMusic(levelMusicHurry);
        }
        else
        {
            ChangeMusic(levelMusic);
        }

        Debug.Log(this.name + " Start: current scene is " + SceneManager.GetActiveScene().name);
    }

    void RetrieveGameState()
    {
        marioSize = t_GameStateManager.marioSize;
        deaths = t_GameStateManager.deaths;
        coins = t_GameStateManager.coins;
        scores = t_GameStateManager.scores;
        timeElapsed = t_GameStateManager.timeElapsed;
        hurryUp = t_GameStateManager.hurryUp;
    }


    /****************** Timer */
    void Update()
    {
        if (!timerPaused)
        {
            timeElapsed += Time.deltaTime; // 1 game sec ~ 0.4 real time sec
            SetHudTime();
        }

       /* if (timeElapsedInt < 100 && !hurryUp)
        { //Todo: Not required. comment it after checking isInvincibleStarman thing
            hurryUp = true;
            PauseMusicPlaySound(warningSound, true);
            if (isInvincibleStarman)
            {  //Todo: I think isInvincibleStarman needs to be outside
                ChangeMusic(starmanMusicHurry, warningSound.length);
            }
            else
            {
                ChangeMusic(levelMusicHurry, warningSound.length);
            }
        }*/

        /*if (timeLeftInt <= 0) {
			MarioRespawn (true);
		}*/

        if (Input.GetButtonDown("Pause"))
        {
            if (!gamePaused)
            {
                StartCoroutine(PauseGameCo());
            }
            else
            {
                StartCoroutine(UnpauseGameCo());
            }
        }

    }


    /****************** Game pause */
    List<Animator> unscaledAnimators = new List<Animator>();
    float pauseGamePrevTimeScale;
    bool pausePrevMusicPaused;

    IEnumerator PauseGameCo()
    {
        gamePaused = true;
        pauseGamePrevTimeScale = Time.timeScale;

        Time.timeScale = 0;
        pausePrevMusicPaused = musicPaused;
        musicSource.Pause();
        musicPaused = true;
        soundSource.Pause();

        // Set any active animators that use unscaled time mode to normal
        unscaledAnimators.Clear();
        foreach (Animator animator in FindObjectsOfType<Animator>())
        {
            if (animator.updateMode == AnimatorUpdateMode.UnscaledTime)
            {
                unscaledAnimators.Add(animator);
                animator.updateMode = AnimatorUpdateMode.Normal;
            }
        }

        pauseSoundSource.Play();
        yield return new WaitForSecondsRealtime(pauseSoundSource.clip.length);
        Debug.Log(this.name + " PauseGameCo stops: records prevTimeScale=" + pauseGamePrevTimeScale.ToString());
    }

    IEnumerator UnpauseGameCo()
    {
        pauseSoundSource.Play();
        yield return new WaitForSecondsRealtime(pauseSoundSource.clip.length);

        musicPaused = pausePrevMusicPaused;
        if (!musicPaused)
        {
            musicSource.UnPause();
        }
        soundSource.UnPause();

        // Reset animators
        foreach (Animator animator in unscaledAnimators)
        {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
        unscaledAnimators.Clear();

        Time.timeScale = pauseGamePrevTimeScale;
        gamePaused = false;
        Debug.Log(this.name + " UnpauseGameCo stops: resume prevTimeScale=" + pauseGamePrevTimeScale.ToString());
    }


    /****************** Invincibility */
    public bool isInvincible()
    {
        return isInvinciblePowerdown || isInvincibleStarman;
    }

    public void MarioInvincibleStarman()
    {
        StartCoroutine(MarioInvincibleStarmanCo());
        AddScore(starmanBonus, mario.transform.position);
    }

    IEnumerator MarioInvincibleStarmanCo()
    {
        isInvincibleStarman = true;
        mario_Animator.SetBool("isInvincibleStarman", true);
        mario.gameObject.layer = LayerMask.NameToLayer("Mario After Starman");
        if (hurryUp)
        {
            ChangeMusic(starmanMusicHurry);
        }
        else
        {
            ChangeMusic(starmanMusic);
        }
        yield return new WaitForSeconds(MarioInvincibleStarmanDuration);
        isInvincibleStarman = false;
        mario_Animator.SetBool("isInvincibleStarman", false);
        mario.gameObject.layer = LayerMask.NameToLayer("Mario");
        if (hurryUp)
        {
            ChangeMusic(levelMusicHurry);
        }
        else
        {
            ChangeMusic(levelMusic);
        }
    }

    void MarioInvinciblePowerdown()
    {
        StartCoroutine(MarioInvinciblePowerdownCo());
    }

    IEnumerator MarioInvinciblePowerdownCo()
    {
        isInvinciblePowerdown = true;
        mario_Animator.SetBool("isInvinciblePowerdown", true);
        mario.gameObject.layer = LayerMask.NameToLayer("Mario After Powerdown");
        yield return new WaitForSeconds(MarioInvinciblePowerdownDuration);
        isInvinciblePowerdown = false;
        mario_Animator.SetBool("isInvinciblePowerdown", false);
        mario.gameObject.layer = LayerMask.NameToLayer("Mario");
    }


    /****************** Powerup / Powerdown / Die */
    public void MarioPowerUp()
    {
        soundSource.PlayOneShot(powerupSound); // should play sound regardless of size
        if (marioSize < 2)
        {
            StartCoroutine(MarioPowerUpCo());
        }
        AddScore(powerupBonus, mario.transform.position);
    }

    IEnumerator MarioPowerUpCo()
    {
        mario_Animator.SetBool("isPoweringUp", true);
        Time.timeScale = 0f;
        mario_Animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        yield return new WaitForSecondsRealtime(transformDuration);
        yield return new WaitWhile(() => gamePaused);

        Time.timeScale = 1;
        mario_Animator.updateMode = AnimatorUpdateMode.Normal;

        marioSize++;
        mario.UpdateSize();
        mario_Animator.SetBool("isPoweringUp", false);
    }

    public void MarioPowerDown(string diedFrom = "default")
    { //called when mario dies by colliding with enemy (So far Brown Goomba, Green Koopa(turtile),  ) and ...
        if (!isPoweringDown)
        {
            Debug.Log(this.name + " MarioPowerDown: called and executed");
            isPoweringDown = true;

            if (marioSize > 0)
            {
                StartCoroutine(MarioPowerDownCo());
                soundSource.PlayOneShot(pipePowerdownSound);
            }
            else
            {
                MarioRespawn(diedFrom);
            }
            Debug.Log(this.name + " MarioPowerDown: done executing");
        }
        else
        {
            Debug.Log(this.name + " MarioPowerDown: called but not executed");
        }
    }

    IEnumerator MarioPowerDownCo()
    {
        mario_Animator.SetBool("isPoweringDown", true);
        Time.timeScale = 0f;
        mario_Animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        yield return new WaitForSecondsRealtime(transformDuration);
        yield return new WaitWhile(() => gamePaused);

        Time.timeScale = 1;
        mario_Animator.updateMode = AnimatorUpdateMode.Normal;
        MarioInvinciblePowerdown();

        marioSize = 0;
        mario.UpdateSize();
        mario_Animator.SetBool("isPoweringDown", false);
        isPoweringDown = false;
    }
  
    /****************** Kill enemy */
    public void MarioStompEnemy(Enemy enemy)
    {
        mario_Rigidbody2D.velocity = new Vector2(mario_Rigidbody2D.velocity.x + stompBounceVelocity.x, stompBounceVelocity.y);
        enemy.StompedByMario();
        soundSource.PlayOneShot(stompSound);
        AddScore(enemy.stompBonus, enemy.gameObject.transform.position);
        Debug.Log(this.name + " MarioStompEnemy called on " + enemy.gameObject.name);
    }

    public void MarioStarmanTouchEnemy(Enemy enemy)
    {
        enemy.TouchedByStarmanMario();
        soundSource.PlayOneShot(kickSound);
        AddScore(enemy.starmanBonus, enemy.gameObject.transform.position);
        Debug.Log(this.name + " MarioStarmanTouchEnemy called on " + enemy.gameObject.name);
    }

    public void RollingShellTouchEnemy(Enemy enemy)
    {
        enemy.TouchedByRollingShell();
        soundSource.PlayOneShot(kickSound);
        AddScore(enemy.rollingShellBonus, enemy.gameObject.transform.position);
        Debug.Log(this.name + " RollingShellTouchEnemy called on " + enemy.gameObject.name);
    }

    public void BlockHitEnemy(Enemy enemy)
    {
        enemy.HitBelowByBlock();
        AddScore(enemy.hitByBlockBonus, enemy.gameObject.transform.position);
        Debug.Log(this.name + " BlockHitEnemy called on " + enemy.gameObject.name);
    }

    public void FireballTouchEnemy(Enemy enemy)
    {
        enemy.HitByMarioFireball();
        soundSource.PlayOneShot(kickSound);
        AddScore(enemy.fireballBonus, enemy.gameObject.transform.position);
        Debug.Log(this.name + " FireballTouchEnemy called on " + enemy.gameObject.name);
    }

    /****************** Scene loading */

    //Sequence of calling
    //MarioPowerDown then MarioRespawn

    // Always called when mario dies in any way 
    public void MarioRespawn(string diedFrom, bool timeup = false)
    {
        //Todo Distinguish feedback types from String "diedFrom"
        if (!isRespawning)
        {
            isRespawning = true;

            marioSize = 0;
            deaths++;
            SetHudDeath();
            soundSource.Stop();
            musicSource.Stop();
            musicPaused = true;
            soundSource.PlayOneShot(deadSound);

            Time.timeScale = 0f; //it stops enemy mario movement when mario dies!
            //mario.Freeze();
            // mario.Die (); //Now this is called after feedback timer is over in "LoadSceneDelayCo"!

            if (timeup)
            {
                Debug.Log(this.name + " MarioRespawn: called due to timeup");
            }
            Debug.Log(this.name + " MarioRespawn: death counts=" + deaths.ToString());

            if (deaths > 0)
            {
                //  ReloadCurrentLevel(diedFrom, deadSound.length, timeup); Old
                ReloadCurrentLevel(diedFrom, loadSceneDelay, timeup);
            }
            else
            {
                //No need of this as lives are infinite so only initiate "ReloadCurrentLevel" 
                /*				LoadGameOver (deadSound.length, timeup);
								Debug.Log(this.name + " MarioRespawn: all dead");*/
            }
        }
    }
  
    public void FeedbackButtonClicked()
    {
        if (feedbackPanelTitleText.text == Constants.FEEDBACK_TITLE_MARIO_DIED_FROM_ENEMY) //From LevelManager
        {
            feedbackPanel.gameObject.SetActive(false);
            StartCoroutine(LoadSceneWhenMarioDied(3));
        }
        else if (feedbackPanelTitleText.text == Constants.FEEDBACK_TITLE_LOST_ENEMY) //from KillPlane
        { 
            feedbackPanel.gameObject.SetActive(false);
            Time.timeScale = 1f;
            //mario.UnfreezeUserInput();
        }
        else if (feedbackPanelTitleText.text == Constants.FEEDBACK_TITLE_MARIO_DIED_FROM_PLANE) //from LevelManager
        {
            feedbackPanel.gameObject.SetActive(false);
            StartCoroutine(LoadSceneWhenMarioDied(3));
        }
        else if (feedbackPanelTitleText.text == Constants.FEEDBACK_TITLE_OUT_OF_SIGHT_ENEMY) { //from MoveAndFlip
            feedbackPanel.gameObject.SetActive(false);
            Time.timeScale = 1f;
 
        }
        else if (feedbackPanelTitleText.text == Constants.FEEDBACK_TITLE_MISSED_COLLECTABLE_BLOCK)
        { //from CollectibleBlock
            feedbackPanel.gameObject.SetActive(false);
            Time.timeScale = 1f;
 
        }
        
        else if (feedbackPanelTitleText.text == Constants.FEEDBACK_TITLE_MISSED_BONUS_LEVEL)
        { //from PipeWarpDown
            feedbackPanel.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        else if (feedbackPanelTitleText.text == Constants.FEEDBACK_TITLE_MISSED_COIN)
        { //from Coin
            feedbackPanel.gameObject.SetActive(false);
            Time.timeScale = 1f;

        }
    }
    //
    public void FeedbackActivaotor(string title, string Description)
    {
        if (!Constants.IS_FEEDBACK_DELAYED && !Constants.NO_FEEDBACK)
        {
            feedbackPanel.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

        feedbackPanelTitleText.text = title; 
        feedbackPanelDecsriptionText.text = Description;
    }
        IEnumerator LoadSceneWhenMarioDied(float delay = 3f)
    {
        mario.Die();
        isRespawning = false;
        isPoweringDown = false;
        float waited = 0;

        while (waited < delay)
        {
            if (!gamePaused)
            { // should not count delay while game paused
                waited += Time.unscaledDeltaTime;

            }
            yield return null;
        }
        yield return new WaitWhile(() => gamePaused);
        SceneManager.LoadScene(t_GameStateManager.sceneToLoad);
    }

    void LoadSceneDelay(string sceneName, float delay = loadSceneDelay)
    {
        t_GameStateManager.delayWhenGamestatesaved = true; //Only using for delayed feedback condition. When Marios dies dont show but when level ends show! 

        timerPaused = true;
        //  if (PipeWarpDown.marioEnteredCount != 0) //Only start coroutine with these conditons!
        StartCoroutine(LoadSceneDelayCo(sceneName, delay));
    }

    IEnumerator LoadSceneDelayCo(string sceneName, float delay)
    {
        Debug.Log(this.name + " LoadSceneDelayCo: starts loading " + sceneName);

        float waited = 0;
        while (waited < delay)
        {
            if (!gamePaused)
            { // should not count delay while game paused
                waited += Time.unscaledDeltaTime;

            }
            yield return null;
        }
        yield return new WaitWhile(() => gamePaused);

        /*       if (feedbackPanel != null)
                   feedbackPanel.gameObject.SetActive(false);
               mario.Die();
               isRespawning = false;
               isPoweringDown = false;*/
        Debug.Log(this.name + sceneName);

        SceneManager.LoadScene(sceneName);

        Debug.Log(this.name + " LoadSceneDelayCo: done loading " + sceneName);
    }

    public void ReloadCurrentLevel(string diedFrom, float delay = loadSceneDelay, bool timeup = false)
    {
        Debug.Log("----- " + diedFrom);
        //Called when mario dies!
        t_GameStateManager.SaveGameState();
        //t_GameStateManager.ConfigReplayedLevel (); //No need. Only setting time!
        t_GameStateManager.sceneToLoad = SceneManager.GetActiveScene().name; //stores current level name. Helps in restating same level

        //todo Imeplement feedback panel + typesbefore reloading the scene
        if (diedFrom == Constants.ENEMY_PLANES)
        {
           FeedbackActivaotor(Constants.FEEDBACK_TITLE_MARIO_DIED_FROM_PLANE, Constants.FEEDBACK_DESCRIPTION_PLANE);
            Constants.FEEDBACK_MARIO_DIED_FROM_PLANE_COUNT++; 
/*            feedbackPanel.gameObject.SetActive(true);
            feedbackPanelTitleText.text = Constants.FEEDBACK_TITLE_MARIO_DIED_FROM_PLANE;
            feedbackPanelDecsriptionText.text = Constants.FEEDBACK_DESCRIPTION_PLANE;*/
        }
        else if (diedFrom == Constants.ENEMY_GOOMBA || diedFrom == Constants.ENEMY_KOOPA)
        {
            FeedbackActivaotor(Constants.FEEDBACK_TITLE_MARIO_DIED_FROM_ENEMY, Constants.FEEDBACK_DESCRIPTION_MARIO_DIED);
            Constants.FEEDBACK_MARIO_DIED_FROM_ENEMY_COUNT++;
/*            feedbackPanel.gameObject.SetActive(true);
            feedbackPanelTitleText.text = Constants.FEEDBACK_TITLE_MARIO_DIED_FROM_ENEMY;
            feedbackPanelDecsriptionText.text = Constants.FEEDBACK_DESCRIPTION_MARIO_DIED;*/
        }
        else
        {
            feedbackPanel.gameObject.SetActive(true);
        }

        //End

        if (timeup)
        {
            LoadSceneDelay("Time Up Screen", delay); //Will NOT called as time is infinite
        }
        else
        {
            LoadSceneDelay("Level Start Screen", delay); //Only this will be called 
        }
    }

    public void LoadNewLevel(string sceneName, float delay = loadSceneDelay)
    {
        t_GameStateManager.SaveGameState();
        t_GameStateManager.ConfigNewLevel();
        t_GameStateManager.sceneToLoad = sceneName;
        LoadSceneDelay("Level Start Screen", delay);
    }

    public void LoadSceneCurrentLevel(string sceneName, float delay = loadSceneDelay)
    {  //Also called when entering pipe down "PipeWarpDown" and ...
        t_GameStateManager.SaveGameState();
        t_GameStateManager.ResetSpawnPosition(); // TODO
        LoadSceneDelay(sceneName, delay);
    }

    public void LoadSceneCurrentLevelSetSpawnPipe(string sceneName, int spawnPipeIdx, float delay = loadSceneDelay)
    { //Also called when entering pipe side "PipeWarpSide" and ...
        t_GameStateManager.SaveGameState();
        t_GameStateManager.SetSpawnPipe(spawnPipeIdx);
        LoadSceneDelay(sceneName, delay);
        Debug.Log(this.name + " LoadSceneCurrentLevelSetSpawnPipe: supposed to load " + sceneName
            + ", spawnPipeIdx=" + spawnPipeIdx.ToString() + "; actual GSM spawnFromPoint="
            + t_GameStateManager.spawnFromPoint.ToString() + ", spawnPipeIdx="
            + t_GameStateManager.spawnPipeIdx.ToString());
    }

    
    public void LoadGameOver(float delay = loadSceneDelay, bool timeup = false)
    {
        // I think this will never called! as game never ends beacuse of infite lives! 

        int currentHighScore = PlayerPrefs.GetInt("highScore", 0);
        if (scores > currentHighScore)
        {
            PlayerPrefs.SetInt("highScore", scores);
        }
        t_GameStateManager.timeup = timeup;
        LoadSceneDelay("Game Over Screen", delay);
    }


    /****************** HUD and sound effects */
    public void SetHudCoin()
    {
        coinText.text = "x" + coins.ToString("D2");
    }

    public void SetHudScore()
    {
        scoreText.text = scores.ToString("D6");
    }

    public void SetHudTime()
    {
        timeElapsedInt = Mathf.RoundToInt(timeElapsed);
        //timeText.text = timeElapsedInt.ToString ("D0"); //Old format

        float minutes = Mathf.FloorToInt(timeElapsedInt / 60);
        float seconds = Mathf.FloorToInt(timeElapsedInt % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetHudDeath()
    {
        deathText.text = deaths.ToString("D0");
    }

    public void CreateFloatingText(string text, Vector3 spawnPos)
    {
        GameObject textEffect = Instantiate(FloatingTextEffect, spawnPos, Quaternion.identity);
        textEffect.GetComponentInChildren<TextMesh>().text = text.ToUpper();
    }


    public void ChangeMusic(AudioClip clip, float delay = 0)
    {
        StartCoroutine(ChangeMusicCo(clip, delay));
    }

    IEnumerator ChangeMusicCo(AudioClip clip, float delay)
    {
        Debug.Log(this.name + " ChangeMusicCo: starts changing music to " + clip.name);
        musicSource.clip = clip;
        yield return new WaitWhile(() => gamePaused);
        yield return new WaitForSecondsRealtime(delay);
        yield return new WaitWhile(() => gamePaused || musicPaused);
        if (!isRespawning)
        {
            musicSource.Play();
        }
        Debug.Log(this.name + " ChangeMusicCo: done changing music to " + clip.name);
    }

    public void PauseMusicPlaySound(AudioClip clip, bool resumeMusic)
    {
        StartCoroutine(PauseMusicPlaySoundCo(clip, resumeMusic));
    }

    IEnumerator PauseMusicPlaySoundCo(AudioClip clip, bool resumeMusic)
    {
        string musicClipName = "";
        if (musicSource.clip)
        {
            musicClipName = musicSource.clip.name;
        }
        Debug.Log(this.name + " PausemusicPlaySoundCo: starts pausing music " + musicClipName + " to play sound " + clip.name);

        musicPaused = true;
        musicSource.Pause();
        soundSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        if (resumeMusic)
        {
            musicSource.UnPause();

            musicClipName = "";
            if (musicSource.clip)
            {
                musicClipName = musicSource.clip.name;
            }
            Debug.Log(this.name + " PausemusicPlaySoundCo: resume playing music " + musicClipName);
        }
        musicPaused = false;

        Debug.Log(this.name + " PausemusicPlaySoundCo: done pausing music to play sound " + clip.name);
    }

    /****************** Game state */
    public void AddLife()
    {
        deaths--;
        SetHudDeath();
        soundSource.PlayOneShot(oneUpSound);
    }

    public void AddLife(Vector3 spawnPos)
    {
        deaths--;
        SetHudDeath();
        soundSource.PlayOneShot(oneUpSound);
        CreateFloatingText("1UP", spawnPos);
    }

    public void AddCoin()
    {
        coins++;
        soundSource.PlayOneShot(coinSound);
        if (coins == 100)
        {
            AddLife();
            coins = 0;
        }
        SetHudCoin();
        AddScore(coinBonus);
    }

    public void AddCoin(Vector3 spawnPos)
    {
        coins++;
        soundSource.PlayOneShot(coinSound);
        if (coins == 100)
        {
            AddLife();
            coins = 0;
        }
        SetHudCoin();
        AddScore(coinBonus, spawnPos);
    }

    public void AddScore(int bonus)
    {
        scores += bonus;
        SetHudScore();
    }

    public void AddScore(int bonus, Vector3 spawnPos)
    {
        scores += bonus;
        SetHudScore();
        if (bonus > 0)
        {
            CreateFloatingText(bonus.ToString(), spawnPos);
        }
    }


    /****************** Misc */
    public Vector3 FindSpawnPosition()
    {
        Vector3 spawnPosition;
        GameStateManager t_GameStateManager = FindObjectOfType<GameStateManager>();
        Debug.Log(this.name + " FindSpawnPosition: GSM spawnFromPoint=" + t_GameStateManager.spawnFromPoint.ToString()
            + " spawnPipeIdx= " + t_GameStateManager.spawnPipeIdx.ToString()
            + " spawnPointIdx=" + t_GameStateManager.spawnPointIdx.ToString());
        if (t_GameStateManager.spawnFromPoint)
        {
            spawnPosition = GameObject.Find("Spawn Points").transform.GetChild(t_GameStateManager.spawnPointIdx).transform.position;
        }
        else
        {
            spawnPosition = GameObject.Find("Spawn Pipes").transform.GetChild(t_GameStateManager.spawnPipeIdx).transform.Find("Spawn Pos").transform.position;
        }
        return spawnPosition;
    }

    public string GetWorldName(string sceneName)
    {
        string[] sceneNameParts = Regex.Split(sceneName, " - ");
        return sceneNameParts[0];
    }

    public bool isSceneInCurrentWorld(string sceneName)
    {
        return GetWorldName(sceneName) == GetWorldName(SceneManager.GetActiveScene().name);
    }

    public void MarioCompleteCastle()
    {
        timerPaused = true;
        ChangeMusic(castleCompleteMusic);
        musicSource.loop = false;
        mario.AutomaticWalk(mario.castleWalkSpeedX);
    }

    public void MarioCompleteLevel()
    {
        timerPaused = true;
        ChangeMusic(levelCompleteMusic);
        musicSource.loop = false;
    }

    public void MarioReachFlagPole()
    {
        timerPaused = true;
        PauseMusicPlaySound(flagpoleSound, false);
        mario.ClimbFlagPole();
    }
}
