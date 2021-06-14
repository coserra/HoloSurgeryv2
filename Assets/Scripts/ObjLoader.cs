using Dummiesman;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;

public class ObjLoader : MonoBehaviour
{
    private string objPath;
    private BoundsControl boundsControl;

    GameObject loadedObj;

    [SerializeField] Material transparentMaterial;
    [SerializeField] Material wireMaterial;
    bool activeTexture;

    [SerializeField] TextMeshPro debugText;

    [SerializeField] GameObject backPlate;

    void Start()
    {
        objPath = GameManager.Instance.fileToOpen;
        boundsControl = GetComponent<BoundsControl>();

        LoadObj();
    }

    void LoadObj()
    {
        //var fs = new FileStream(objPath,FileMode.Open);
        //OBJLoader objLoader = new OBJLoader();
        //objLoader.SplitMode = SplitMode.None;

        //loadedObj = objLoader.Load(fs);

        loadedObj = new GameObject();

        Mesh holderMesh = new Mesh();
        ObjImporter newMesh = new ObjImporter();
        holderMesh = newMesh.ImportFile(objPath);
        
        MeshRenderer renderer = loadedObj.AddComponent<MeshRenderer>();
        MeshFilter filter = loadedObj.AddComponent<MeshFilter>();
        filter.mesh = holderMesh;
        renderer.material = wireMaterial;
        activeTexture = false;

        BoxCollider collider = loadedObj.AddComponent<BoxCollider>();
        FitColliderToChildren(loadedObj);
        loadedObj.transform.parent = gameObject.transform;
        loadedObj.transform.localScale = new Vector3(0.000000001f, 0.000000001f, 0.000000001f);
        //loadedObj.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        boundsControl.BoundsOverride = collider;
        backPlate.transform.position = collider.bounds.min + new Vector3(collider.bounds.size.x/2,-backPlate.transform.localScale.y,collider.bounds.size.z/2);
    }

    public void ChangeTexture()
    {
        //if (activeTexture)
        //{
        //    loadedObj.GetComponent<MeshRenderer>().material = wireMaterial;
        //}
        //else
        //{
        //    loadedObj.GetComponent<MeshRenderer>().material = transparentMaterial;
        //}
        //activeTexture = !activeTexture;
    }

    public void prueba2()
    {
        var www = new WWW("https://people.sc.fsu.edu/~jburkardt/data/obj/lamp.obj");
        while (!www.isDone)
            System.Threading.Thread.Sleep(1);

        //create stream and load
        var textStream = new MemoryStream(Encoding.UTF8.GetBytes(www.text));
        var loadedObj = new OBJLoader().Load(textStream);
    }

    public void prueba3()
    {
        Debug.Log(Application.temporaryCachePath);
        debugText.text = Application.temporaryCachePath;

        if (File.Exists(Path.Combine(Application.temporaryCachePath, "objeto.obj")))
        {
            var loadedObj = new OBJLoader().Load(Path.Combine(Application.temporaryCachePath, "objeto.obj"));
        }
    }


    private void FitColliderToChildren(GameObject parentObject)
    {
        BoxCollider bc = parentObject.GetComponent<BoxCollider>();
        if (bc == null) { bc = parentObject.AddComponent<BoxCollider>(); }
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        bool hasBounds = false;
        Renderer[] renderers = parentObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer render in renderers)
        {
            if (hasBounds)
            {
                bounds.Encapsulate(render.bounds);
            }
            else
            {
                bounds = render.bounds;
                hasBounds = true;
            }
        }
        if (hasBounds)
        {
            bc.center = bounds.center - parentObject.transform.position;
            bc.center = new Vector3(bc.center.x, bc.center.y, bc.center.z);
            bc.size = bounds.size;
            
        }
        else
        {
            bc.size = bc.center = Vector3.zero;
            bc.size = Vector3.zero;
        }
    }
}
