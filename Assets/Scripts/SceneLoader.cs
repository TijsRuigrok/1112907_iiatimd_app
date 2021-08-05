using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private int currentSceneIndex;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    /// <summary>
    /// Loads specific scene based on scene name.
    /// </summary>
    /// <param name="sceneName">Name of requested scene.</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Reloads current scene.
    /// </summary>
    public void ReloadScene()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

    /// <summary>
    /// Quits application. Does not work in editor.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
