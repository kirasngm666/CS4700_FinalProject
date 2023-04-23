
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CameraControl : MonoBehaviour
{
    public Transform player;
    public Transform nextSection;

    private GameManagerController gameManager;
    private bool isPlayerTeleported = false;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManagerController.instance;
    }

    // Update is called once per frame
    void Update()
    {
       
        //if 4 enemies have been beaten, move camera to boss section
       if (GameManagerController.instance != null && GameManagerController.instance.enemiesBeaten >= 4)
        {
            

            if (!isPlayerTeleported)
            {
                Vector2 newPosition = new Vector2(nextSection.position.x - 3f, nextSection.position.y);
                player.position = newPosition;
                isPlayerTeleported = true;
            }
            transform.position = new Vector2(player.position.x, player.position.y);

        }
        //if no boss fight, camera follows player
        else
        {
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }
    }
}