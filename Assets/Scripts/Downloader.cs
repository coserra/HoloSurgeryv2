using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine.Networking;
using System.IO;
using System.Text;
using System.Threading.Tasks;
#if WINDOWS_UWP
using Windows.Storage;
#endif

public class Downloader : MonoBehaviour
{
    [SerializeField] TextMeshPro texto;
    [SerializeField] ProgressIndicatorLoadingBar loadingBar;
    [SerializeField] TextMeshPro loadingText;
    [SerializeField] TextMeshPro debugText;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject aceptPanel;
    [SerializeField] GameObject importPanel;
    [SerializeField] GameObject errorPanel;

    private string savePath;

    private void Start()
    {
        texto.text = GameManager.Instance.info;
    }

    public void StartDownload()
    {
        Uri downloadAddress = new Uri(GameManager.Instance.info);
        string filename = System.IO.Path.GetFileName(downloadAddress.LocalPath);
        StartCoroutine(GetFile(downloadAddress, filename));
    }

    public void CancelDownload()
    {
        GameManager.Instance.LoadOnlyThisScene("QRDetection");
    }

    public void UpdateLoadingBar(long current, long total)
    {
        loadingBar.Progress=(float)current / total;
    }

    public void UpdateLoadingBar(float progress)
    {
        loadingBar.Progress = (float) progress;
    }

    IEnumerator GetFile(Uri downloadUrl, string filename)
    {
        Debug.Log("Descarga de " + downloadUrl + " en " + Path.Combine(Application.persistentDataPath, filename));
        UnityWebRequest www = new UnityWebRequest(downloadUrl);
        www.downloadHandler = new DownloadHandlerBuffer();
        StartCoroutine(DownloadInfo(www));
        aceptPanel.SetActive(false);
        yield return www.SendWebRequest();


        Debug.Log(www.downloadHandler.text);
        debugText.text = www.downloadHandler.text;
        bool downloadOK;
        if (www.error != null)
        {
            downloadOK = false;
            Debug.Log(www.error);
            debugText.text = www.error;
        }
        else
        {
            downloadOK = true;
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
            savePath = Path.Combine(Application.persistentDataPath, filename);
            //Now Save it
            System.IO.File.WriteAllBytes(savePath, results);
        }

        if (downloadOK)
        {
            importPanel.SetActive(true);
        }
        else
        {
            errorPanel.SetActive(true);
        }
    }

    IEnumerator DownloadInfo(UnityWebRequest uwr)
    {
        loadingPanel.SetActive(true);
        loadingBar.OpenAsync();
        
        while (!uwr.isDone)
        {
            UpdateLoadingBar(uwr.downloadProgress);
            yield return null;
        }
        loadingBar.CloseAsync();
        loadingPanel.SetActive(false);
        loadingText.text = "Descarga finalizada";
    }

    public void PostDownload()
    {
        GameManager.Instance.QuickLoad(savePath);
    }
}
