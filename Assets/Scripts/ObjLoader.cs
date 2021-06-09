using Dummiesman;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjLoader : MonoBehaviour
{
    private string objPath;
    private BoundsControl boundsControl;

    [SerializeField] GameObject backPlate;

    void Start()
    {
        objPath = GameManager.Instance.fileToOpen;
        boundsControl = GetComponent<BoundsControl>();

        LoadObj();
    }

    void LoadObj()
    {
        GameObject loadedObj = new OBJLoader().Load(objPath);
        BoxCollider collider = loadedObj.AddComponent<BoxCollider>();
        FitColliderToChildren(loadedObj);
        loadedObj.transform.parent = gameObject.transform;
        loadedObj.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        boundsControl.BoundsOverride = collider;
        backPlate.transform.position = collider.bounds.min + new Vector3(collider.bounds.size.x/2,-backPlate.transform.localScale.y,collider.bounds.size.z/2);
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
            bc.center = new Vector3(-bc.center.x, bc.center.y, bc.center.z);
            bc.size = bounds.size;
            
        }
        else
        {
            bc.size = bc.center = Vector3.zero;
            bc.size = Vector3.zero;
        }
    }
}
