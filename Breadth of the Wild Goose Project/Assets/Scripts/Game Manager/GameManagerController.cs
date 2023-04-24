using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerController : MonoBehaviour
{
    public int enemiesBeaten = 0;

    public static GameManagerController instance;

    private bool isActive;

    private GameObject rightWall;

    public bool bossDefeated = false;

    public void Awake()
    {
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Start() 
    {
        rightWall = GameObject.FindGameObjectWithTag("Wall Right");
    }
    void Update()
    {
        if (!isActive) 
        {
            GetRidOfWall();
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
        }
    }

    public void BossDefeated()
    {
        bossDefeated = true;
    }
}
