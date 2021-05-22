using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCoordinator : MonoBehaviour
{
    [SerializeField] Transform coordinatedWith;

    BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        coordinatedWith.localPosition = gameObject.transform.localPosition;
        coordinatedWith.localScale = gameObject.transform.localScale;
        coordinatedWith.rotation = gameObject.transform.rotation;
        //boxCollider.size = gameObject.transform.localScale;
    }
}
