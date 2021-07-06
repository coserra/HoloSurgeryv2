using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialViewController : MonoBehaviour
{
    [SerializeField] RadialView radialView;

    private void Start()
    {
        if (radialView == null) radialView = GetComponent<RadialView>();
    }

    public void ChangeState()
    {
        radialView.enabled = !radialView.enabled;
    }
}
