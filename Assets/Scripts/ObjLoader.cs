using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LoadObj()
    {
        Mesh holderMesh = new Mesh();
        //ObjImporter newMesh = new ObjImporter();
        //holderMesh = newMesh.ImportFile("C:/Users/cvpa2/Desktop/ng/output.obj");

        MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
        MeshFilter filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = holderMesh;
    }
}
