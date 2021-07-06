using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollController : MonoBehaviour
{
    [SerializeField]
    private ScrollingObjectCollection scrollView;
    [SerializeField] GameObject container;
    [SerializeField] GameObject gridCollection;

    [SerializeField] float tierLenght;
    [SerializeField] int tierLimit;
    public int tier;
    
    private void Start()
    {
        tier = 0;
    }

    public void ScrollByTier(int amount)
    {
        scrollView.MoveByTiers(amount);
    }

    public void ScrollDown()
    {
        if (tier < (gridCollection.transform.childCount/2-tierLimit)/2)
        {
            float position = container.transform.position.y;
            StartCoroutine(LerpScroll(position, position + tierLenght));
            tier++;
        }
        
    }

    public void ScrollUp()
    {
        if (tier > 0)
        {
            float position = container.transform.position.y;
            StartCoroutine(LerpScroll(position, position - tierLenght));
            tier--;
        }
    }

    private IEnumerator LerpScroll(float start, float end)
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.01f);
            container.transform.position = new Vector3(container.transform.position.x, Mathf.Lerp(start, end, i / 10f), container.transform.position.z);
        }
    }
}
