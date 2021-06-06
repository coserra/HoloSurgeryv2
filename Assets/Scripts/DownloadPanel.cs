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

public class DownloadPanel : MonoBehaviour
{
    [SerializeField] TextMeshPro texto;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] ProgressIndicatorLoadingBar loadingBar;
    [SerializeField] TextMeshPro loadingText;
    [SerializeField] TextMeshPro debugText;

    WWWDownloader www;

    private void Start()
    {
        texto.text = GameManager.Instance.info;
        www = gameObject.GetComponent<WWWDownloader>();
    }

    public void StartDownload()
    {
        Uri downloadAddress = new Uri(GameManager.Instance.info);
        StartCoroutine(GetFile(downloadAddress, "archivo.zip"));
        //www.DownloadFile(GameManager.Instance.info, "archivo.zip");
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

    IEnumerator GetFile(Uri downloadUrl, string filename)
    {
        Debug.Log("Descarga de " + downloadUrl + " en " + Path.Combine(Application.persistentDataPath, filename));
        FileDownloader fl = new FileDownloader();
        UnityWebRequest www = new UnityWebRequest(downloadUrl);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
        debugText.text = www.downloadHandler.text;
        if (www.error != null)
        {
            Debug.Log(www.error);
            debugText.text = www.error;
        }
        else
        {
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
            string savePath = Path.Combine(Application.persistentDataPath, filename);
            //Now Save it
            System.IO.File.WriteAllBytes(savePath, results);
        }
    }
}
