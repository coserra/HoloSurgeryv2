using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeRotation : MonoBehaviour
{
    [SerializeField] Transform mainCam;
    [SerializeField] Transform model;
    [SerializeField] Transform plane;

    [SerializeField] float vectorSize;

    // Update is called once per frame
    void LateUpdate()
    {
        gameObject.transform.position = (mainCam.position - plane.position).normalized*vectorSize+model.position;
        gameObject.transform.LookAt(model);
    }
}
