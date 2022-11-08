using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILight : MonoBehaviour
{

    void Update()
    {
        foreach(Transform child in transform)
        {
            if(child.GetChild(0).Find("Light").gameObject.active == false)
            {
                child.GetChild(0).Find("Light").gameObject.SetActive(true);
            }
        }
    }
}
