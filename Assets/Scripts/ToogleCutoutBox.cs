using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToogleCutoutBox : MonoBehaviour
{
    [SerializeField] GameObject realBox;
    [SerializeField] GameObject fakeBox;
    [SerializeField] GameObject container;

    private BoxCollider containerCollider;
    private ObjectManipulator containerManipulator;
    private BoundsControl containerControl;

    private bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        containerCollider = container.GetComponent<BoxCollider>();
        containerManipulator = container.GetComponent<ObjectManipulator>();
        containerControl = container.GetComponent<BoundsControl>();
    }

    public void ChangeStatus()
    {
        Debug.Log("Cambiando");
        if (isActive)
        {
            realBox.SetActive(false);
            fakeBox.SetActive(false);
            containerCollider.enabled =true;
            containerManipulator.enabled =true;
            containerControl.enabled =true;
        }
        else
        {
            realBox.SetActive(true);
            fakeBox.SetActive(true);
            containerCollider.enabled =false;
            containerManipulator.enabled =false;
            containerControl.enabled =false;
        }
        isActive = !isActive;
    }
}
