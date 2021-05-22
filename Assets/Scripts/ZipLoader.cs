using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZipLoader : MonoBehaviour
{

    FileDownloader fileDownloader;
    // Start is called before the first frame update
    void Start()
    {
        FileDownloader fileDownloader = new FileDownloader();

        // This callback is triggered for DownloadFileAsync only
        fileDownloader.DownloadProgressChanged += (sender, e) => Debug.Log("Progress changed " + e.BytesReceived + " " + e.TotalBytesToReceive);
        // This callback is triggered for both DownloadFile and DownloadFileAsync
        fileDownloader.DownloadFileCompleted += (sender, e) => Debug.Log("Download completed");

        //fileDownloader.DownloadFileAsync("https://drive.google.com/file/d/1_u9UWNOvtBi8KB3skLcq6pwnyAEyCezS/view?usp=sharing", @"C:\Users\Pablo\papeles\unex\cuarto\TFG\archivo.zip");
    }


}
