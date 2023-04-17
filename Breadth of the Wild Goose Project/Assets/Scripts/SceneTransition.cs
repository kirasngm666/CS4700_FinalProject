//<<<<<<< Updated upstream
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public string nextSceneName;
    public GameObject transitionCanvas;

    public int targetDisplay = 1; //change to this display


    private bool transitionInProgress = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !transitionInProgress)
        {
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition()
    {
        transitionInProgress = true;

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(nextSceneName);
        loadOperation.allowSceneActivation = false;

        // Display your transition graphics here
        transitionCanvas.SetActive(true);

        while (loadOperation.progress < 0.9f)
        {
            yield return null;
        }

        loadOperation.allowSceneActivation = true;

        // Wait for the new scene to finish loading before unloading the transition scene
        while (!loadOperation.isDone)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync("TransitionScene");

        transitionInProgress = false;

        //change to display 2
        Display.displays[targetDisplay].Activate();
    }
}
/*
//=======
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public string nextSceneName;
    public GameObject transitionCanvas;

    public int targetDisplay = 1; //change to this display


    private bool transitionInProgress = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !transitionInProgress)
        {
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition()
    {
        transitionInProgress = true;

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(nextSceneName);
        loadOperation.allowSceneActivation = false;

        // Display your transition graphics here
        transitionCanvas.SetActive(true);

        while (loadOperation.progress < 0.9f)
        {
            yield return null;
        }

        loadOperation.allowSceneActivation = true;

        // Wait for the new scene to finish loading before unloading the transition scene
        while (!loadOperation.isDone)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync("TransitionScene");

        transitionInProgress = false;

        //change to display 2
        Display.displays[targetDisplay].Activate();
    }
//>>>>>>> Stashed changes
}
*/