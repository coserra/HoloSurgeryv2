using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowURI : MonoBehaviour
{
    [SerializeField] TextMeshPro texto;


    private void Start()
    {
        texto.text = GameManager.Instance.info;
    }
}
