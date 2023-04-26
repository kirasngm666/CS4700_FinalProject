using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Initialize component
    
    private GameObject player;
    //public Transform player;
    public Transform nextSection;
    public float smoothTime = 1.0f;
    private GameManagerController gameManagerController;
    private GameObject gameManager;

    // Bool values
    private bool isPlayerTeleported = false;
    
    // Other Info 
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        gameManagerController = gameManager.GetComponent<GameManagerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //if 4 enemies have been beaten, move camera to boss section
        if (GameManagerController.instance != null && GameManagerController.instance.enemiesBeaten >= 10)
        {
           
            if (!isPlayerTeleported)
            {
                Invoke("Teleportation", 75/100);
                //gameManagerController.BringUpTheWall();
                isPlayerTeleported = true;
            }

            transform.position = Vector3.SmoothDamp(transform.position, nextSection.position, ref velocity, smoothTime);
        }
        //if no boss fight, camera follows player
        else
        {
            //transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.transform.position.z);
        }
    }

    void Teleportation()
    {
        Vector3 newPosition = new Vector3(nextSection.position.x - 3f, nextSection.position.y, nextSection.position.z);
        player.transform.position = newPosition;
    }
}
