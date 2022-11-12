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
    public bool isHoldingObj;
    public static PickupThrow instance;

    void Awake() 
    {
        instance = this;    
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && isHoldingObj)
        {
            isHoldingObj = false;
            obj.transform.SetParent(null);
            obj.GetComponent<Rigidbody>().isKinematic = false;
            obj.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Extrapolate;
            obj.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            obj.GetComponent<Rigidbody>().AddForce(transform.parent.forward * throwForce);      
            obj = null;      

            StartCoroutine(MouseLookScript.instance.CanHoverAgain());
        }
    }

    public void PickupObj(GameObject go)
    {
        obj = go;
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
        go.GetComponent<Rigidbody>().velocity = Vector3.zero;
        go.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        
        isHoldingObj = true;
    }
}
