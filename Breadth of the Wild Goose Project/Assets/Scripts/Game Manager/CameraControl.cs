using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Initialize component
    public Transform player;
    public Transform nextSection;
    public float smoothTime = 0.3f;
    private GameManagerController gameManagerController;
    private GameObject gameManager;

    // Bool values
    private bool isPlayerTeleported = false;
    
    // Other Info 
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
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
                Vector3 newPosition = new Vector3(nextSection.position.x - 3f, nextSection.position.y, nextSection.position.z);
                player.position = newPosition;
                //gameManagerController.BringUpTheWall();
                isPlayerTeleported = true;
            }

            transform.position = Vector3.SmoothDamp(transform.position, nextSection.position, ref velocity, smoothTime);
        }
        //if no boss fight, camera follows player
        else
        {
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }
    }
}
