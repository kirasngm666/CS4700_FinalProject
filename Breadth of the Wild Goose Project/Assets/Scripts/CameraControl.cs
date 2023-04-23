using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public Transform nextSection;
    public Transform secondBackground;
    public float smoothTime = 0.3f;

    private GameManagerController gameManager;
    private bool hasReachedNextSection = false;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManagerController.instance;
    }

    // Update is called once per frame
    void Update()
    {
        //if 4 enemies have been beaten and the player hasn't reached the next section yet
        if (GameManagerController.instance != null && GameManagerController.instance.enemiesBeaten >= 4 && !hasReachedNextSection)
        {
            //if the player hasn't reached the right wall yet, move towards it
            if (player.position.x < nextSection.position.x - 3f)
            {
                player.Translate(new Vector3(1f, 0f, 0f) * Time.deltaTime);
            }
            //if the player has reached the right wall, move towards the next section
            else
            {
                hasReachedNextSection = true;
            }
        }

        //if the player has reached the next section, move towards it and then towards the second background
        if (hasReachedNextSection)
        {
            transform.position = Vector3.SmoothDamp(transform.position, nextSection.position, ref velocity, smoothTime);

            //if the camera has reached the next section, move towards the second background
            if (transform.position.x >= nextSection.position.x - 0.1f)
            {
                transform.position = Vector3.SmoothDamp(transform.position, secondBackground.position, ref velocity, smoothTime);
            }
        }
        //if no boss fight and the player hasn't reached the next section yet, camera follows player
        else
        {
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }
    }
}
