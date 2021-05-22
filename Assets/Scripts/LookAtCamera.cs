using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] GameObject camera;


    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.LookAt(camera.transform);
        gameObject.transform.rotation= Quaternion.LookRotation(transform.position - camera.transform.position);
    }
}
