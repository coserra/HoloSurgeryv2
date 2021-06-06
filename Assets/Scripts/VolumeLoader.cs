using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityVolumeRendering;

public class VolumeLoader : MonoBehaviour
{
    string volumePath;
    private VolumeRenderedObject volume;

    [SerializeField] CutoutBox cutoutBox;

    void Start()
    {
        if (!Path.HasExtension(GameManager.Instance.fileToOpen))
        {
            volumePath = GameManager.Instance.fileToOpen;
        }

        RenameFilesExtension();
        ImportDicom();
    }
    private void RenameFilesExtension()
    {
        DirectoryInfo directory = new DirectoryInfo(volumePath);
        FileInfo[] files = directory.GetFiles();
        foreach (FileInfo file in files)
        {
            if (!file.Name.EndsWith(".dcm"))
            {
                Path.ChangeExtension(file.FullName, ".dcm");
            }
        }
    }


    private void ImportDicom()
    {
        DirectoryInfo directory = new DirectoryInfo(volumePath);
        FileInfo[] dicomFiles = directory.GetFiles();
        string[] dicomPaths = new string[dicomFiles.Length];
        for (int i = 0; i < dicomFiles.Length; i++)
        {
            dicomPaths[i] = dicomFiles[i].FullName;
        }

        Debug.Log("Importando dataset");
        DICOMImporter importer = new DICOMImporter(dicomPaths);
        VolumeDataset volumeDataset = importer.Import();
        volume = VolumeObjectFactory.CreateObject(volumeDataset);
        volume.transform.parent = gameObject.transform;
        volume.transform.localPosition = Vector3.zero;
        SetLayerRecursively(volume.gameObject, 8);
        cutoutBox.targetObject = volume;
        //volumeArea.gameObject.AddComponent<BoxCollider>();
        //volumeArea.gameObject.AddComponent<ObjectManipulator>();
        Debug.Log("Dataset importado");
    }


    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
