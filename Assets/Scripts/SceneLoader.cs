using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void LoadScene(string sceneName)
    {
        gameManager.LoadScene(sceneName);
    }

    public void LoadOnlyThisScene(string sceneName)
    {
        gameManager.LoadOnlyThisScene(sceneName);
    }

    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
