using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



//we can attach this script to a button object in the scene and this will start the transition coroutine
//when button is pressed, a loading scene is shown before the next scene is fully loaded

public class SceneTransition : MonoBehaviour
{
    public string sceneName; //the next scene name here
    public GameObject transition;

    public void LoadNextScene()
    {
        StartCoroutine(Transition());
    }

    private IEnumerator Transition()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("put next scene name here"); //LoadSceneAsync will allow seamless transitions 
        load.allowSceneActivation = false;

        transition.SetActive(true);

        while(load.progress < 0.9f)
        {
            yield return null;
        }

        load.allowSceneActivation = true;

        while (!load.isDone)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync("put transition scene name here");
    }
}
