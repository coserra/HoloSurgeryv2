using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MySingleton<GameManager>
{
    public Camera mainCamera;
    [SerializeField] GameObject loadingPanel;

    List<AsyncOperation> _loadOperations;
    private Hashtable _currentSceneLoaded;
    //private List<Scene> _currentSceneLoaded;
    public int lastSceneLoaded;
    public string downloadPath { set; get; }
    public string info { set; get; }

    public string fileToOpen { set; get; }

    public Action<string> QuickLoadEvent;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _loadOperations = new List<AsyncOperation>();
        _currentSceneLoaded = new Hashtable();
        lastSceneLoaded = 0;

        //info = "https://i.blogs.es/397d0c/zelda-breath-of-the-wild-01/1366_2000.jpg";
        //info = "http://s628528467.mialojamiento.es/wp-content/3dForNAS.rar";
        info = "http://s628528467.mialojamiento.es/wp-content/uploads/2021/06/MedicalContentForNAS.zip";
        //info = "https://drive.google.com/file/d/1_u9UWNOvtBi8KB3skLcq6pwnyAEyCezS/view?usp=sharing";
        //info = "https://drive.google.com/uc?export=download&confirm=swMM&id=1_u9UWNOvtBi8KB3skLcq6pwnyAEyCezS";
        //info = "https://drive.google.com/uc?id=1_u9UWNOvtBi8KB3skLcq6pwnyAEyCezS&export=download";

        fileToOpen = "";
    }
    
    void OnLoadOperationComplete(AsyncOperation ao)
    {
        loadingPanel.SetActive(false);
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);
        }
        Debug.Log("Scene load completed");

        lastSceneLoaded++;
        _currentSceneLoaded.Add(lastSceneLoaded, SceneManager.GetSceneAt(SceneManager.sceneCount-1));
        
        foreach (int n in _currentSceneLoaded.Keys)
        {
            Scene s = (Scene) _currentSceneLoaded[n];
            Debug.Log("Numero " + n + " escena: " + s.name);
        }
    }

    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        Debug.Log("Scene unload completed");
    }

    public void LoadScene(string sceneName)
    {
        loadingPanel.SetActive(true);
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
        if (ao == null)
        {
            return;
        }
        ao.completed += OnLoadOperationComplete;
        _loadOperations.Add(ao);
        
    }

    public void LoadOnlyThisScene(string sceneName)
    {
        foreach(Scene s in _currentSceneLoaded.Values)
        {
            UnloadScene(s);
        }
        _currentSceneLoaded.Clear();
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
        //_currentSceneLoaded.Remove(sceneName);
    }

    public void UnloadAll()
    {
        foreach (Scene s in _currentSceneLoaded.Values)
        {
            UnloadScene(s);
        }
        _currentSceneLoaded.Clear();
    }

    public void UnloadScene(Scene scene)
    {
        Debug.Log("Unloading " + scene.name);
        AsyncOperation ao = SceneManager.UnloadSceneAsync(scene);
        if (ao == null)
        {
            return;
        }
        ao.completed += OnUnloadOperationComplete;
        //_currentSceneLoaded.Remove(scene);
    }

    public void UnloadScene(int sceneId)
    {
        Scene scene = (Scene) _currentSceneLoaded[sceneId];
        Debug.Log("Unloading " + scene.name);
        AsyncOperation ao = SceneManager.UnloadSceneAsync(scene);
        if (ao == null)
        {
            return;
        }
        ao.completed += OnUnloadOperationComplete;
        _currentSceneLoaded.Remove(sceneId);
    }

    public void QuickLoad(string path)
    {
        if (QuickLoadEvent != null) QuickLoadEvent.Invoke(path);
    }

    private void OnApplicationQuit()
    {
        DirectoryInfo dir = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "tmp"));
        if (dir.Exists)
            dir.Delete(true);
    }

    public void CloseApplication()
    {
        Debug.Log("Cerrando aplicación");
        Application.Quit();
    }
}
