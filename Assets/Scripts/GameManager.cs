using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MySingleton<GameManager>
{
    public Camera mainCamera;

    List<AsyncOperation> _loadOperations;
    private List<string> _currentSceneLoaded;
    public string downloadPath { set; get; }
    public string info { set; get; }

    public string fileToOpen { set; get; }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _loadOperations = new List<AsyncOperation>();
        _currentSceneLoaded = new List<string>();
        LoadScene("menu");

        //TODO enlace perfectamente desde QR
        info = "http://s628528467.mialojamiento.es/wp-content/3dForNAS.rar";
        //info = "https://drive.google.com/file/d/1_u9UWNOvtBi8KB3skLcq6pwnyAEyCezS/view?usp=sharing";
        //info = "https://drive.google.com/uc?export=download&confirm=swMM&id=1_u9UWNOvtBi8KB3skLcq6pwnyAEyCezS";
        //info = "https://drive.google.com/uc?id=1_u9UWNOvtBi8KB3skLcq6pwnyAEyCezS&export=download";

        fileToOpen = @"C:\Users\Pablo\AppData\LocalLow\DefaultCompany\HoloSurgeryv20\4k-the-legend-of-zelda-breath-of-the-wild-characters-2910.jpg";
    }
    
    void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);
        }
        Debug.Log("Scene load completed");
    }

    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        Debug.Log("Scene unload completed");
    }

    public void LoadScene(string sceneName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
        if (ao == null)
        {
            return;
        }
        ao.completed += OnLoadOperationComplete;
        _loadOperations.Add(ao);
        _currentSceneLoaded.Add(sceneName);
    }

    public void LoadOnlyThisScene(string sceneName)
    {
        while (_currentSceneLoaded.Count > 0)
        {
            string scene = _currentSceneLoaded[0];
            UnloadScene(scene);
        }
        LoadScene(sceneName);
    }

    public void UnloadScene(string sceneName)
    {
        Debug.Log("Unloading " + sceneName);
        AsyncOperation ao = SceneManager.UnloadSceneAsync(sceneName);
        if (ao == null)
        {
            return;
        }
        ao.completed += OnUnloadOperationComplete;
        _currentSceneLoaded.Remove(sceneName);
    }


}
