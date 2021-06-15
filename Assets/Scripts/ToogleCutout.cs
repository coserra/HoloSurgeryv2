using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToogleCutout : MonoBehaviour
{
    [SerializeField] GameObject realBox;
    [SerializeField] GameObject fakeBox;
    [SerializeField] GameObject realPlane;
    [SerializeField] GameObject fakePlane;
    [SerializeField] GameObject container;

    private BoxCollider containerCollider;
    private ObjectManipulator containerManipulator;
    private BoundsControl containerControl;

    // Start is called before the first frame update
    void Start()
    {
        containerCollider = container.GetComponent<BoxCollider>();
        containerManipulator = container.GetComponent<ObjectManipulator>();
        containerControl = container.GetComponent<BoundsControl>();
    }

    public void ChangeBoxStatus()
    {
        if (fakeBox.activeSelf)
        {
            realBox.SetActive(false);
            fakeBox.SetActive(false);
            containerCollider.enabled =true;
            containerManipulator.enabled =true;
            containerControl.enabled =true;
        }
        else
        {
            if (fakePlane.activeSelf) ChangePlaneStatus();
            realBox.SetActive(true);
            fakeBox.SetActive(true);
            containerCollider.enabled =false;
            containerManipulator.enabled =false;
            containerControl.enabled =false;
        }
    }

    public void ChangePlaneStatus()
    {
        if (fakePlane.activeSelf)
        {
            realPlane.SetActive(false);
            fakePlane.SetActive(false);
            containerCollider.enabled = true;
            containerManipulator.enabled = true;
            containerControl.enabled = true;
        }
        else
        {
            if (fakeBox.activeSelf) ChangeBoxStatus();
            realPlane.SetActive(true);
            fakePlane.SetActive(true);
            containerCollider.enabled = false;
            containerManipulator.enabled = false;
            containerControl.enabled = false;
        }
    }
}
