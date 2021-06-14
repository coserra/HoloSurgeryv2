using Microsoft.MixedReality.Toolkit.UI;
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
    [SerializeField] CrossSectionPlane crossSectionPlane;

    [SerializeField] GameObject slicingQuad1;
    [SerializeField] GameObject slicingQuad2;
    [SerializeField] GameObject slicingQuad3;

    SlicingPlane slicingPlane1;
    SlicingPlane slicingPlane2;
    SlicingPlane slicingPlane3;

    private float min1;
    private float max1;
    private float min2;
    private float max2;
    private float min3;
    private float max3;

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
        crossSectionPlane.targetObject = volume;
        //volumeArea.gameObject.AddComponent<BoxCollider>();
        //volumeArea.gameObject.AddComponent<ObjectManipulator>();
        Debug.Log("Dataset importado");
        CreateSlicingPlane();
    }


    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public void UpdateVisibilityMin(SliderEventData data)
    {
        if (volume != null)
        {
            float max = volume.GetVisibilityWindow().y;
            volume.SetVisibilityWindow(data.NewValue, max);
        }
        
    }

    public void UpdateVisibilityMax(SliderEventData data)
    {
        if (volume != null)
        {
            float min = volume.GetVisibilityWindow().x;
            volume.SetVisibilityWindow(min, data.NewValue);
        }
    }

    public void CreateSlicingPlane()
    {
        slicingPlane1 = volume.CreateSlicingPlane();
        slicingPlane2 = volume.CreateSlicingPlane();
        slicingPlane3 = volume.CreateSlicingPlane();
        
        SetLayerRecursively(slicingPlane1.gameObject, 9);
        SetLayerRecursively(slicingPlane2.gameObject, 9);
        SetLayerRecursively(slicingPlane3.gameObject, 9);
        slicingQuad1.transform.localScale = slicingPlane1.GetComponent<Renderer>().bounds.size;
        slicingQuad2.transform.localScale = slicingPlane2.GetComponent<Renderer>().bounds.size;
        slicingQuad3.transform.localScale = slicingPlane3.GetComponent<Renderer>().bounds.size;

        slicingQuad1.GetComponent<MeshRenderer>().material = slicingPlane1.GetComponent<MeshRenderer>().material;
        slicingQuad2.GetComponent<MeshRenderer>().material = slicingPlane2.GetComponent<MeshRenderer>().material;
        slicingQuad3.GetComponent<MeshRenderer>().material = slicingPlane3.GetComponent<MeshRenderer>().material;

        slicingPlane2.transform.Rotate(Vector3.forward, 270);
        slicingPlane3.transform.Rotate(Vector3.right, 90);

    }

    public void MoveSlicingPlane1(SliderEventData data)
    {
        if (slicingPlane1 != null)
        {
            Vector3 position = slicingPlane1.transform.position;
            slicingPlane1.transform.localPosition = new Vector3(0, data.NewValue - 0.5f, 0);
        }
    }

    public void MoveSlicingPlane2(SliderEventData data)
    {
        if (slicingPlane2 != null)
        {
            Vector3 position = slicingPlane2.transform.position;
            slicingPlane2.transform.localPosition = new Vector3(data.NewValue - 0.5f, 0, 0);
        }
    }

    public void MoveSlicingPlane3(SliderEventData data)
    {
        if (slicingPlane3 != null)
        {
            Vector3 position = slicingPlane3.transform.position;
            slicingPlane3.transform.localPosition = new Vector3(0, 0, -(data.NewValue - 0.5f));
        }
    }

}
