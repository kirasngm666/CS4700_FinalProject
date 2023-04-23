using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public Transform nextSection;
    public float smoothTime = 0.3f;

    private GameManagerController gameManager;
    private bool isPlayerTeleported = false;
    private Vector3 velocity = Vector3.zero;

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
                Vector3 newPosition = new Vector3(nextSection.position.x - 3f, nextSection.position.y, nextSection.position.z);
                player.position = newPosition;
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
