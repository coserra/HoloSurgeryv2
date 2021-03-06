using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
#if WINDOWS_UWP
    using Windows.Storage;
    using Windows.Storage.Pickers;
    using Windows.UI.ViewManagement;
    using System.Runtime.InteropServices;
#endif

public class FilePickerManager
{
    private string path;
    //[SerializeField] TextMeshPro texto;
    public Action<string> onFolderPicked;
    public Action<string> onFilePicked;

    public void PickFile()
    {
#if !UNITY_EDITOR && UNITY_WSA_10_0
        Debug.Log("***********************************");
        Debug.Log("File Picker start.");
        //texto.text += "File Picker start.";
        Debug.Log("***********************************");

        UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
        {
            var filepicker = new FileOpenPicker();
            // filepicker.FileTypeFilter.Add("*");
            filepicker.FileTypeFilter.Add(".zip");
            filepicker.FileTypeFilter.Add(".obj");
            filepicker.FileTypeFilter.Add(".mp4");
            filepicker.FileTypeFilter.Add(".jpg");
            filepicker.FileTypeFilter.Add(".png");

            var file = await filepicker.PickSingleFileAsync();
            await file.CopyAsync(ApplicationData.Current.LocalFolder);
            UnityEngine.WSA.Application.InvokeOnAppThread(() => 
            {
                Debug.Log("***********************************");
                string name = (file != null) ? file.Name : "No data";
                Debug.Log("Name: " + name);
                //texto.text += "Name: " + name;
                Debug.Log("***********************************");
                path = (file != null) ? file.Path : "No data";
                Debug.Log("Path: " + path);
                //texto.text += "Path: " + path;
                Debug.Log("***********************************");

                //This section of code reads through the file (and is covered in the link)
                // but if you want to make your own parcing function you can 
                if (onFilePicked != null)
                    onFilePicked.Invoke(name);

            }, false);
        }, false);

        
        Debug.Log("***********************************");
        Debug.Log("File Picker end.");
        Debug.Log("***********************************");
#else
        Debug.Log("Ejecutado desde el editor de Unity");
        path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        path=Path.Combine(path, "archivo.zip");
        if (onFilePicked != null)
            onFilePicked.Invoke(path);
        //texto.text = "url de prueba";
#endif
    }

    public void PickFolder()
    {
#if !UNITY_EDITOR && UNITY_WSA_10_0
        Debug.Log("***********************************");
        Debug.Log("Folder Picker start.");
        //texto.text += "Folder Picker start.";
        Debug.Log("***********************************");

        UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
        {
            var folderpicker = new FolderPicker();
            folderpicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderpicker.FileTypeFilter.Add("*");

            var folder = await folderpicker.PickSingleFolderAsync();
            UnityEngine.WSA.Application.InvokeOnAppThread(() => 
            {
                Debug.Log("***********************************");
                string name = (folder != null) ? folder.Name : "No data";
                Debug.Log("Name: " + name);
                //texto.text += "Name: " + name;
                Debug.Log("***********************************");
                path = (folder != null) ? folder.Path : "No data";
                Debug.Log("Path: " + path);
                //texto.text += "Path: " + path;
                Debug.Log("***********************************");

                if(onFolderPicked!=null)
                    onFolderPicked.Invoke(path);
            }, false);
        }, false);

        
        Debug.Log("***********************************");
        Debug.Log("Folder Picker end.");
        Debug.Log("***********************************");
#else
        Debug.Log("Ejecutado desde el editor de Unity");
        path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        path=Path.Combine(path, "Downloads");
        if (onFilePicked != null)
            onFilePicked.Invoke(path);
        //texto.text = "url de prueba";
#endif
    }
}
