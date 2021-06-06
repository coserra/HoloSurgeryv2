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

    private List<GameObject> instances;

    public event Action<int> OnReturn;

    // Start is called before the first frame update
    void Start()
    {
        instances = new List<GameObject>();
    }

    public void SetNewList(List<string> list)
    {
        DestroyList();
        for (int i=0;i<list.Count;i++)
        {
            var instance = Instantiate(buttonPrefab, grid.transform);
            ButtonConfigHelper bch = instance.GetComponent<ButtonConfigHelper>();
            bch.MainLabelText = list[i];
            bch.SeeItSayItLabelEnabled = false;
            int temp = i;
            bch.OnClick.AddListener(() => ItemSelected(temp));
            Debug.Log("En el listener se ha colocado el numero " + i);
            instances.Add(instance);
        }
        grid.GetComponent<GridObjectCollection>().UpdateCollection();
    }

    public void DestroyList()
    {
        foreach (GameObject g in instances)
        {
            Destroy(g);
        }
        instances.Clear();
        grid.GetComponent<GridObjectCollection>().UpdateCollection();
    }

    public void ItemSelected(int selection)
    {
        if (OnReturn!=null)
            OnReturn.Invoke(selection);
    }
}
