using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEngine;

public class Loader : MonoBehaviour
{
    private List<string> pathList;
    private List<string> pathNames;

    private ListController listController;

    // Start is called before the first frame update
    void Start()
    {
        listController = GetComponent<ListController>();
        listController.OnReturn += ItemSelected;
        pathList = new List<string>();
        pathNames = new List<string>();
    }

    public void LoadPersistentDataPath()
    {
        pathList.Clear();
        pathNames.Clear();
        LoadDataPath(Application.persistentDataPath);
        if (pathNames.Count>0)
            listController.SetNewList(pathNames);
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
}
