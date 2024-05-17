using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [Header("Transition")]
    public Animator transition;
    public float transitionTime = 1f;

    /// <summary>
    /// Coroutine used to take actions when loading a scene.
    /// </summary>
    /// <param name="i">Scene number.</param>
    /// <returns></returns>
    IEnumerator LoadLevel(int i)
    {
        //play anim
        transition.SetTrigger("Start");
        //wait for anim
        yield return new WaitForSeconds(transitionTime);
        //load scenes
        AsyncOperation operation = SceneManager.LoadSceneAsync(i);

        // Wait until the scene is fully loaded
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Loads a new scene.
    /// </summary>
    /// <param name="i">Scene number.</param>
    public void PlayGame(int i)
    {
        StartCoroutine(LoadLevel(i));

    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
