using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageLoader : MonoBehaviour
{
    // Start is called before the first frame update
    private string imagePath;
    private SpriteRenderer sprite;

    void Start()
    {
        if (GameManager.Instance.fileToOpen.EndsWith("jpg")|| GameManager.Instance.fileToOpen.EndsWith("png"))
        {
            imagePath = GameManager.Instance.fileToOpen;
        }
        sprite = gameObject.GetComponent<SpriteRenderer>();
        LoadImage();
    }


    private void LoadImage()
    {
        if (File.Exists(imagePath))
        {
            byte[] fileData = File.ReadAllBytes(imagePath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            sprite.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 20000.0f);
        }
    }
}
