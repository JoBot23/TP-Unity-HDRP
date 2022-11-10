using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupThrow : MonoBehaviour
{

    public Material outlinedMaterial;
    public float pickupRange;
    private GameObject obj;
    public static PickupThrow instance;

    void Awake() 
    {
        instance = this;    
    }

    void Update()
    {
        
    }

    public void PickupObj(GameObject go)
    {
        print(go.transform.parent.name);
    }
}
