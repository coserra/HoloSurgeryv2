using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using TMPro;
using UnityEngine;

public class Loader : MonoBehaviour
{
    private List<string> pathList;
    private List<string> pathNames;

    private ListController listController;

    [SerializeField] GameObject list;
    [SerializeField] GameObject importPanel;
    [SerializeField] TextMeshPro debugtext;

    private string lastImportedPath;

    // Start is called before the first frame update
    void Start()
    {
        listController = GetComponent<ListController>();
        pathList = new List<string>();
        pathNames = new List<string>();
        GameManager.Instance.QuickLoadEvent += QuickLoad;
    }

    public void LoadPersistentDataPath()
    {
        pathList.Clear();
        pathNames.Clear();
        LoadDataPath(Application.persistentDataPath);
        if (pathNames.Count > 0)
        {
            listController.ClearReturnEvent();
            listController.OnReturn += ItemSelected;
            listController.SetNewList(pathNames);
        }
    }
    private void LoadDataPath(string path)
    {
        DirectoryInfo directory = new DirectoryInfo(path);
        Debug.Log("Contando archivos en " + directory.FullName);
        DirectoryInfo[] directoryList = directory.GetDirectories();
        FileInfo[] files = directory.GetFiles("*",SearchOption.AllDirectories).Where(s => AceptedFormat(s.Name)).ToArray();

        foreach (FileInfo file in files)
        {
            if (AceptedFormat(file.Name))
            {
                Debug.Log(file.Name);
                pathList.Add(file.FullName);
                pathNames.Add(file.Name);
            }
        }
    }


    public void ItemSelected(int folderNumber)
    {
        Debug.Log("Seleccionado " + pathNames[folderNumber] + " (" + pathList[folderNumber] + " )");
        if (pathNames[folderNumber].EndsWith("zip"))
        {
            string zipDestinyPath = Path.Combine(Application.persistentDataPath, "tmp");
            zipDestinyPath = Path.Combine(zipDestinyPath, pathNames[folderNumber].Substring(0, pathNames[folderNumber].IndexOf(".")));

            ExtractZip(pathList[folderNumber], zipDestinyPath);
            SelectUsefulFolders(zipDestinyPath);

        }
        else if (pathNames[folderNumber].EndsWith("jpg")|| pathNames[folderNumber].EndsWith("png"))
        {
            Debug.Log(pathList[folderNumber]);
            GameManager.Instance.fileToOpen = pathList[folderNumber];
            GameManager.Instance.LoadScene("Image");
        }
        else if (pathNames[folderNumber].EndsWith("mp4"))
        {
            GameManager.Instance.fileToOpen = pathList[folderNumber];
            GameManager.Instance.LoadScene("Video");
        }
        else if (pathNames[folderNumber].EndsWith("obj"))
        {
            GameManager.Instance.fileToOpen = pathList[folderNumber];
            GameManager.Instance.LoadScene("Obj");
        }
        else
        {
            if (!Path.HasExtension(pathNames[folderNumber]))
            {
                GameManager.Instance.fileToOpen = pathList[folderNumber];
                GameManager.Instance.LoadScene("VolumeRenderInPlane");
            }
        }
    }

    public void QuickLoad()
    {
        QuickLoad(lastImportedPath);
    }

    public void QuickLoad(string path)
    {
        string filename = Path.GetFileName(new Uri(path).LocalPath);
        Debug.Log("Seleccionado " + filename + " (" + path + " )");
        if (filename.EndsWith("zip"))
        {
            string zipDestinyPath = Path.Combine(Application.persistentDataPath, "tmp");
            zipDestinyPath = Path.Combine(zipDestinyPath, filename.Substring(0, filename.IndexOf(".")));

            list.SetActive(true);

            ExtractZip(path, zipDestinyPath);
            SelectUsefulFolders(zipDestinyPath);

        }
        else if (filename.EndsWith("jpg") || filename.EndsWith("png"))
        {
            Debug.Log(path);
            GameManager.Instance.fileToOpen = path;
            GameManager.Instance.LoadScene("Image");
        }
        else if (filename.EndsWith("mp4"))
        {
            GameManager.Instance.fileToOpen = path;
            GameManager.Instance.LoadScene("Video");
        }
        else if (filename.EndsWith("obj"))
        {
            GameManager.Instance.fileToOpen = path;
            GameManager.Instance.LoadScene("Obj");
        }
        else
        {
            if (!Path.HasExtension(filename))
            {
                GameManager.Instance.fileToOpen = path;
                GameManager.Instance.LoadScene("VolumeRenderInPlane");
            }
        }
    }

    private bool AceptedFormat(string name)
    {
        return (name.EndsWith("rar") || name.EndsWith("zip") || name.EndsWith("jpg") || name.EndsWith("png") || name.EndsWith("mp4") ||
            name.EndsWith("wav") || name.EndsWith("obj"));
    }


    private void ExtractZip(string zipSourcePath, string zipDestinyPath)
    {
        
        Debug.Log(zipDestinyPath);

        // Open an existing zip file for reading
        ZipStorer zip = ZipStorer.Open(zipSourcePath, FileAccess.Read);

        // Read the central directory collection
        List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();

        // Look for the desired file
        foreach (ZipStorer.ZipFileEntry entry in dir)
        {
                //Debug.Log(Path.GetFileName(entry.FilenameInZip));
                // File found, extract it
                zip.ExtractFile(entry, Path.Combine(zipDestinyPath, entry.FilenameInZip));
        }
        zip.Close();

        //using (var unzip = new Internals.Unzip(zipSourcePath))
        //{
        // extract all files from zip archive to a directory
        //    unzip.ExtractToDirectory(zipDestinyPath);
        //}
    }

    public void SelectUsefulFolders(string zipDestinyPath)
    {
        pathList.Clear();
        pathNames.Clear();
        SelectUsefulFoldersRec(zipDestinyPath);
        DirectoryInfo directory = new DirectoryInfo(zipDestinyPath);
        FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories).Where(s => AceptedFormat(s.Name)).ToArray();
        foreach (FileInfo file in files)
        {
            pathList.Add(file.FullName);
            pathNames.Add(file.Name);
        }
        listController.ClearReturnEvent();
        listController.OnReturn += ItemSelected;
        listController.SetNewList(pathNames);
    }

    private void SelectUsefulFoldersRec(string path)
    {
        DirectoryInfo directory = new DirectoryInfo(path);
        Debug.Log("Contando archivos en " + directory.FullName);
        DirectoryInfo[] directoryList = directory.GetDirectories();
        FileInfo[] files = directory.GetFiles();
        if (directoryList.Length == 0)
        {
            if (files.Length > 40)
            {
                pathList.Add(path);
                pathNames.Add(directory.Name);
                Debug.Log(path + " contiene archivos suficientes, añadiendo a la lista");
            }
        }
        else
        {
            foreach (DirectoryInfo d in directoryList)
            {
                SelectUsefulFoldersRec(d.FullName);
            }
        }
    }

    public void Import()
    {
        FilePickerManager fp = new FilePickerManager();
        fp.onFilePicked += CopyFileToPersistentPath;
        fp.PickFile();
    }

    public void CopyFileToPersistentPath(string file)
    {
        lastImportedPath = Path.Combine(Application.persistentDataPath, file);
        importPanel.SetActive(true);
    }

    public void SelectFileToDelete()
    {
        pathList.Clear();
        pathNames.Clear();
        LoadDataPath(Application.persistentDataPath);
        if (pathNames.Count > 0)
        {
            listController.ClearReturnEvent();
            listController.OnReturn += DeleteFile;
            listController.SetNewList(pathNames);
        }
    }

    private void DeleteFile(int folderNumber)
    {
        FileInfo file = new FileInfo(pathList[folderNumber]);
        if (file.Exists)
            file.Delete();
        SelectFileToDelete();
    }
}
