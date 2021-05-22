using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ListController : MonoBehaviour
{
    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject buttonPrefab;

    public event Action<int> OnReturn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetNewList(List<string> list)
    {
        for (int i=0;i<list.Count;i++)
        {
            var instance = Instantiate(buttonPrefab, grid.transform);
            instance.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMesh>().text = list[i];
            int temp = i;
            instance.GetComponent<Interactable>().OnClick.AddListener(()=>ItemSelected(temp));
            Debug.Log("En el listener se ha colocado el numero " + i);
        }
        grid.GetComponent<GridObjectCollection>().UpdateCollection();
    }

    public void DestroyList()
    {

    }

    public void ItemSelected(int selection)
    {
        if (OnReturn!=null)
            OnReturn.Invoke(selection);
    }
}
