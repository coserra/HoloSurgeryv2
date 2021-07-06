using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private GameManager gameManager;
    private int sceneId;

    private void Start()
    {
        gameManager = GameManager.Instance;
        sceneId = gameManager.lastSceneLoaded;
    }

    public void LoadScene(string sceneName)
    {
        gameManager.LoadScene(sceneName);
    }

    public void LoadOnlyThisScene(string sceneName)
    {
        gameManager.LoadOnlyThisScene(sceneName);
    }

    public void CloseThisScene()
    {
        gameManager.UnloadScene(sceneId);
    }

    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
