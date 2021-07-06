using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeFaker : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    private Transform camTransform;
    [SerializeField] Transform model;
    [SerializeField] GameObject plane;
    [SerializeField] Transform container;
    [SerializeField] GameObject secondCam;

    [SerializeField] float vectorSize;


    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera=GameManager.Instance.mainCamera;
        }
        camTransform = mainCamera.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        plane.transform.LookAt(mainCamera.transform);
        plane.transform.rotation = Quaternion.LookRotation(transform.position - camTransform.position);

        model.rotation = container.rotation;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        secondCam.transform.position = (camTransform.position - plane.transform.position).normalized * vectorSize + model.position;
        secondCam.transform.LookAt(model);
    }
}
