using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine.Networking;
using System.IO;

public class DownloadPanel : MonoBehaviour
{
    [SerializeField] TextMeshPro texto;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] ProgressIndicatorLoadingBar loadingBar;
    [SerializeField] TextMeshPro loadingText;

    private void Start()
    {
        texto.text = GameManager.Instance.info;
    }

    public void StartDownload()
    {
        FileDownloader fileDownloader = new FileDownloader();
        GameManager gm = GameManager.Instance;

        //loadingPanel.SetActive(true);
        //gameObject.SetActive(false);
        //loadingBar.OpenAsync();

        // This callback is triggered for DownloadFileAsync only
        //fileDownloader.DownloadProgressChanged += (sender, e) => UpdateLoadingBar(e.BytesReceived, e.TotalBytesToReceive);
        // This callback is triggered for both DownloadFile and DownloadFileAsync
        //fileDownloader.DownloadFileCompleted += (sender, e) => EndDownload();
        //string fullPath=gm.downloadPath +"\\"+ System.DateTime.UtcNow.ToString("ddMMyy_HHmmss") +".zip";
        //Debug.Log("Ruta de descarga:"+fullPath);
        //fileDownloader.DownloadFileAsync(texto.text, fullPath);

        //WWWDownloader downloader = new WWWDownloader();
        //downloader.DownloadImage("https://i.blogs.es/594843/chrome/450_1000.jpg");

        DownloadFile();
    }

    public void CancelDownload()
    {
        GameManager.Instance.LoadOnlyThisScene("QRDetection");
    }

    public void UpdateLoadingBar(long current, long total)
    {
        loadingBar.Progress=(float)current / total;
    }

    public void EndDownload()
    {
        loadingBar.CloseAsync();
        loadingBar.gameObject.SetActive(false);
        loadingText.text = "Descarga finalizada";
    }

    void DownloadFile()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        FileDownloader fl = new FileDownloader();
        UnityWebRequest www = new UnityWebRequest(fl.GetGoogleDriveDownloadAddress("https://drive.google.com/file/d/104tEB6djMJAdAFor-W4cJuxeP47VSHxI/view?usp=sharing"));
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);

        // Or retrieve results as binary data
        byte[] results = www.downloadHandler.data;
        string savePath = Path.Combine(GameManager.Instance.downloadPath, "archivo.txt");
        //Now Save it
        System.IO.File.WriteAllBytes(savePath, results);
        //}
    }
}
