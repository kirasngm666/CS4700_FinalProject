using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerController : MonoBehaviour
{
    
    public static GameManagerController instance;
    private GameObject rightWall;

    private bool isActive;
    public bool bossDefeated = false;
    private bool isPassingTriggerPoint;
    public int enemiesBeaten = 0;

    public static GameManagerController Instance = null;

    bool isGameOver;
    bool playerReady;
    bool initReadyScreen;

    float gameRestartTime;
    float gamePlayerReadyTime;

    public float gameRestartDelay = 5f;
    public float gamePlayerReadyDelay = 3f;

    TextMeshProUGUI warningMessage;

    private GameObject player;
    private GameObject cameraObject;
    //public Transform nextSectionTransform;
    private CameraControl cameraControl;
    private GameObject nextSection;

    private Vector3 velocity = Vector3.zero;

    public void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this
        if (Instance == null)
        {
            Instance = this;
        }
        // If an instance already exists, destroy whatever this object is to enforce the singleton
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Set GameManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Start() 
    {
        rightWall = GameObject.FindGameObjectWithTag("Wall Right");
        player = GameObject.FindGameObjectWithTag("Player");
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        nextSection = GameObject.FindGameObjectWithTag("Next Section Point");
        cameraControl = cameraObject.GetComponent<CameraControl>();
    }
    
    void Update()
    {
        if (!isActive) 
        {
            GetRidOfWall();
        }

        // player ready screen - wait the delay time and show READY on screen
        if (playerReady && enemiesBeaten >= 10)
        {
            // initialize objects and set READY text
            if (initReadyScreen)
            {
                //FreezePlayer(true);
                //FreezeEnemies(true);
                warningMessage.alignment = TextAlignmentOptions.Center;
                warningMessage.alignment = TextAlignmentOptions.Top;
                warningMessage.fontStyle = FontStyles.UpperCase;
                warningMessage.fontSize = 24;
                warningMessage.text = "\n\n\n\nTELEPORT TO THE BIG BOSS";
                initReadyScreen = false;
            }
            // countdown READY screen pause
            gamePlayerReadyTime -= Time.deltaTime;
            if (gamePlayerReadyTime < 0)
            {
                Vector3 newPosition = new Vector3(nextSection.transform.position.x - 3f, nextSection.transform.position.y, nextSection.transform.position.z);
                player.transform.position = newPosition;
                //cameraControl.TeleportCamera();
                cameraObject.transform.position = nextSection.transform.position;
                //FreezePlayer(false);
                //FreezeEnemies(false);
                warningMessage.text = "";
                playerReady = false;
            }
            return;
        }

        // // show player score
        // if (playerScoreText != null)
        // {
        //     playerScoreText.text = String.Format("<mspace=\"{0}\">{1:0000000}</mspace>", playerScoreText.fontSize, playerScore);
        // }

        // if the game isn't over then spawn enemies
        if (!isGameOver)
        {
            // here is where we can do things while the game is running
        }
        else
        {
            // game over, wait delay then reload scene
            gameRestartTime -= Time.deltaTime;
            if (gameRestartTime < 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void EnemyDefeated()
    {
        enemiesBeaten++;
    }

    public void GetRidOfWall()
    {
        if (enemiesBeaten >= 10)
        {
            isActive = true;
            Debug.Log("You've beaten 10 enemies!");
            Debug.Log("Bring down the right wall barrier!");
            rightWall.SetActive(false);
            //warningText.SetActive(true);
        }
    }

    public void BossDefeated()
    {
        bossDefeated = true;
    }

    public void BringUpTheWall()
    {
        isActive = false;
        enemiesBeaten = 0;
        rightWall.SetActive(true);
    }

    // called first
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called when the game is terminated
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // called second
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Do Startup Functions - Scene has to be fully loaded
        // otherwise we can't get a handle on the player score text object
        StartGame();
    }

    // initializes and starts the game
    private void StartGame()
    {
        isGameOver = false;
        playerReady = true;
        initReadyScreen = true;
        gamePlayerReadyTime = gamePlayerReadyDelay;
        //playerScoreText = GameObject.Find("PlayerScore").GetComponent<TextMeshProUGUI>();
        warningMessage = GameObject.Find("WarningMessage").GetComponent<TextMeshProUGUI>();
        //SoundManager.Instance.MusicSource.Play();
    }

    // objects that offer score points should call this method upon their defeat to add to the player's score
    // public void AddScorePoints(int points)
    // {
    //     playerScore += points;
    // }

    // private void FreezePlayer(bool freeze)
    // {
    //     // freeze player and input
    //     GameObject player = GameObject.FindGameObjectWithTag("Player");
    //     if (player != null)
    //     {
    //         player.GetComponent<GooseController>().FreezeInput(freeze);
    //         player.GetComponent<GooseController>().FreezePlayer(freeze);
    //     }
    // }

    // private void FreezeEnemies(bool freeze)
    // {
    //     // freeze all enemies
    //     GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //     foreach (GameObject enemy in enemies)
    //     {
    //         enemy.GetComponent<EnemyController>().FreezeEnemy(freeze);
    //     }
    // }

    // private void FreezeBullets(bool freeze)
    // {
    //     // freeze all bullets
    //     GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
    //     foreach (GameObject bullet in bullets)
    //     {
    //         bullet.GetComponent<BulletScript>().FreezeBullet(freeze);
    //     }
    // }

    public void PlayerDefeated()
    {
        // game over :(
        isGameOver = true;
        gameRestartTime = gameRestartDelay;
        // stop all sounds
        //SoundManager.Instance.Stop();
        //SoundManager.Instance.StopMusic();
        // freeze player and input
        //FreezePlayer(true);
        // freeze all enemies
        //FreezeEnemies(true);
        // remove all bullets
        //GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        // foreach (GameObject bullet in bullets)
        // {
        //     Destroy(bullet);
        // }
        // remove all explosions
        // GameObject[] explosions = GameObject.FindGameObjectsWithTag("Explosion");
        // foreach (GameObject explosion in explosions)
        // {
        //     Destroy(explosion);
        // }
    }
}
