using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WWWDownloader : MonoBehaviour
{

// ...

public Image image;

// ...

public void DownloadImage(string url)
{
    StartCoroutine(ImageRequest(url, (UnityWebRequest req) =>
    {
        if (req.isNetworkError || req.isHttpError)
        {
            Debug.Log($"{req.error}: {req.downloadHandler.text}");
        }
        else
        {
            // Get the texture out using a helper downloadhandler
            Texture2D texture = DownloadHandlerTexture.GetContent(req);
            // Save it into the Image UI's sprite
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }));
}

IEnumerator ImageRequest(string url, Action<UnityWebRequest> callback)
{
    using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(url))
    {
        yield return req.SendWebRequest();
        callback(req);
    }
}
}