using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookScript : MonoBehaviour
{
    public PlayerController player;
    public float mouseSensitivity;
    public Transform playerBody;
    float xRotation = 0f;
    GameObject obj;
    bool outlined;
    bool canHover = true;
    public static MouseLookScript instance;

    void Awake()
    {
        instance = this;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Mouse look
        if (player.canMove)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }


        //Pickup System
        Vector3 direction = Vector3.forward;
        Ray ray = new Ray(transform.position, transform.TransformDirection(direction * PickupThrow.instance.pickupRange));
        Debug.DrawRay(transform.position, transform.TransformDirection(direction * PickupThrow.instance.pickupRange));

        if(Physics.Raycast(ray, out RaycastHit hit, PickupThrow.instance.pickupRange) && canHover)
        {
            if(hit.collider.tag == "Item")
            {
                if(Input.GetKeyDown(KeyCode.E) && outlined)
                {
                    canHover = false;
                    UnOutlineObject();
                    PickupThrow.instance.PickupObj(hit.collider.transform.parent.gameObject);
                    return;
                }

                //Show Outline
                if(!outlined && hit.collider.gameObject != obj)
                {
                    print("in");
                    outlined = true;
                    obj = hit.collider.gameObject;

                    Material[] mats = hit.collider.gameObject.GetComponent<MeshRenderer>().materials;
                    mats[1] = PickupThrow.instance.outlinedMaterial;
                    hit.collider.gameObject.GetComponent<MeshRenderer>().materials = mats;
                }
            }
            else 
            {   
                //Hide Outline
                if(outlined && !PickupThrow.instance.isHoldingObj) 
                {
                    UnOutlineObject();
                }
            }
        }
    }

    public void UnOutlineObject()
    {
        print("out");
        outlined = false;
        Material[] mats = obj.GetComponent<MeshRenderer>().materials;
        mats[1] = PickupThrow.instance.outlinedHiddenMaterial;
        obj.GetComponent<MeshRenderer>().materials = mats;
        obj = null;
    }

    public IEnumerator CanHoverAgain()
    {
        yield return new WaitForSeconds(0.4f);
        canHover = true;
    }
}
