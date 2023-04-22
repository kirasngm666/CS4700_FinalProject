using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerController : MonoBehaviour
{
    [SerializeField] int enemiesBeaten = 0;

    private bool isActive;

    private GameObject rightWall;

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
        if (enemiesBeaten == 4)
        {
            isActive = true;
            Debug.Log("You've beaten 4 enemies!");
            Debug.Log("Bring down the right wall barrier!");
            rightWall.SetActive(false);
        }
    }
}
