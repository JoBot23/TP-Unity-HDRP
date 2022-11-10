using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupThrow : MonoBehaviour
{

    public Material outlinedMaterial;
    public Material outlinedHiddenMaterial;
    public float pickupRange;
    public float throwForce;
    private GameObject obj;
    bool isHoldingObj;
    public static PickupThrow instance;

    void Awake() 
    {
        instance = this;    
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && isHoldingObj)
        {
            StartCoroutine(ThrowObject());
        }
    }

    public void PickupObj(GameObject go)
    {
        obj = go;
        //obj.GetComponentInChildren<Collider>().enabled = false;
        obj.layer = 7;
        obj.transform.SetParent(transform);
        obj.transform.localPosition = new Vector3(0,0,0);
        obj.transform.localRotation = new Quaternion(0,0,0,0);
        
        MeshRenderer[] meshs = GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mesh in meshs)
            mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        if(!obj.GetComponent<Rigidbody>())
            obj.AddComponent<Rigidbody>();

        go.GetComponent<Rigidbody>().isKinematic = true;
        
        isHoldingObj = true;
    }

    private IEnumerator ThrowObject()
    {
        isHoldingObj = false;
        obj.transform.SetParent(null);
        obj.GetComponent<Rigidbody>().isKinematic = false;
        obj.GetComponent<Rigidbody>().AddForce(transform.parent.forward * throwForce);
        
        yield return new WaitForSeconds(0.1f);
        obj.GetComponentInChildren<Collider>().enabled = true;
    } 
}
