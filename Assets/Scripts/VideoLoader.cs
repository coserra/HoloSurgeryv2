using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoLoader : MonoBehaviour
{
    private string videoPath;
    private VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        videoPath = GameManager.Instance.fileToOpen;
    
        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        LoadVideo();
    }

    private void LoadVideo()
    {
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoPath;
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += ResizeQuad;
        
        videoPlayer.Play();
    }

    private void ResizeQuad(VideoPlayer source)
    {
        Texture tex = source.texture;
        float resolution = (float) tex.width / tex.height;
        Debug.Log(resolution);
        gameObject.transform.localScale = new Vector3(resolution, 1, 1);
    }
}
