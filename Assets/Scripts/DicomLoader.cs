using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityVolumeRendering;

public class DicomLoader : MonoBehaviour
{

    private string zipSourcePath;
    private string zipDestinyPath;

    private ListController listController;

    private List<string> dicomPathList;
    private List<string> dicomPathNames;
    [SerializeField] private GameObject volumeArea;
    [SerializeField] private GameObject cutoutBox;
    private GameObject cutBoxInstance;
    [SerializeField] private GameObject crossSectionPlane;
    private VolumeRenderedObject volume;

    //Estados de la aplicación
    private bool boxActive;

    // Start is called before the first frame update
    void Start()
    {
        listController = GetComponent<ListController>();
        listController.OnReturn += ItemSelected;
        dicomPathList = new List<string>();
        dicomPathNames = new List<string>();
        zipDestinyPath = @"C:\Users\Pablo\papeles\unex\cuarto\TFG\DICOM samples\TAC Tórax - 20201221";
        Debug.Log(zipDestinyPath);

        boxActive = false;
    }



    private void ExtractZip()
    {
        using (var unzip = new Internals.Unzip(zipSourcePath))
        {
            // extract all files from zip archive to a directory
            unzip.ExtractToDirectory(zipDestinyPath);
        }
    }

    public void SelectUsefulFolders()
    {
        SelectUsefulFoldersRec(zipDestinyPath);
        listController.SetNewList(dicomPathNames);
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
                dicomPathList.Add(path);
                dicomPathNames.Add(directory.Name);
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

    public void ItemSelected(int folderNumber)
    {
        Debug.Log("Seleccionado " + dicomPathNames[folderNumber]+" ("+dicomPathList[folderNumber]+" )");
        RenameFilesExtension(dicomPathList[folderNumber]);
        ImportDicom(dicomPathList[folderNumber]);

    }

    private void RenameFilesExtension(string path)
    {
        DirectoryInfo directory = new DirectoryInfo(path);
        FileInfo[] files = directory.GetFiles();
        foreach(FileInfo file in files)
        {
            if (!file.Name.EndsWith(".dcm"))
            {
                FileUtils.Rename(file, file.Name + ".dcm");
            }
        }
    }

    private void ImportDicom(string path)
    {
        DirectoryInfo directory = new DirectoryInfo(path);
        FileInfo[] dicomFiles = directory.GetFiles();
        string[] dicomPaths = new string[dicomFiles.Length];
        for(int i =0;i<dicomFiles.Length;i++)
        {
            dicomPaths[i] = dicomFiles[i].FullName;
        }

        Debug.Log("Importando dataset");
        DICOMImporter importer = new DICOMImporter(dicomPaths);
        VolumeDataset volumeDataset = importer.Import();
        volume = VolumeObjectFactory.CreateObject(volumeDataset);
        volume.transform.parent = volumeArea.transform;
        volume.transform.localPosition=Vector3.zero;
        volumeArea.gameObject.AddComponent<BoxCollider>();
        volumeArea.gameObject.AddComponent<ObjectManipulator>();
        Debug.Log("Dataset importado");
    }

    public void AddCrossSectionPlane()
    {
        
    }

    public void AddBoxCutout()
    {
        if (!boxActive)
        {
            if (volume != null)
            {
                cutBoxInstance = Instantiate(cutoutBox, volumeArea.transform);
                cutBoxInstance.GetComponent<CutoutBox>().targetObject = volume;
                volumeArea.GetComponent<ObjectManipulator>().enabled = false;
                volumeArea.GetComponent<BoxCollider>().enabled = false;
                cutBoxInstance.gameObject.AddComponent<BoxCollider>();
                cutBoxInstance.gameObject.AddComponent<ObjectManipulator>();
                cutBoxInstance.gameObject.AddComponent<BoundsControl>();
                boxActive = true;
            }
        }
        else
        {
            volumeArea.GetComponent<ObjectManipulator>().enabled = true;
            volumeArea.GetComponent<BoxCollider>().enabled = true;
            Destroy(cutBoxInstance);
            boxActive = false;
        }
    }

    private void OnApplicationQuit()
    {
        Destroy(volume);
    }
}
