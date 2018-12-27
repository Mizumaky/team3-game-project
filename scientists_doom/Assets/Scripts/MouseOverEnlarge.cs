using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverEnlarge : MonoBehaviour
{
    public void PointerEnter()
    {
        GetComponent<RectTransform>().localScale = 1.1f * Vector3.one;
    }

    public void PointerExit()
    {
        GetComponent<RectTransform>().localScale = Vector3.one;
    }
}
