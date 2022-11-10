using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookScript : MonoBehaviour
{
    public PlayerController player;
    public float mouseSensitivity;
    public Transform playerBody;
    float xRotation = 0f;
    MeshRenderer obj;

    void Start()
    {
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

        if(Physics.Raycast(ray, out RaycastHit hit, PickupThrow.instance.pickupRange))
        {
            if(hit.collider.tag == "Item")
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    PickupThrow.instance.PickupObj(hit.collider.gameObject);
                }

                //Item Show Outline
                if(hit.collider.gameObject.GetComponent<MeshRenderer>().materials.Length == 1)
                {
                    List<Material> mats = new List<Material>(hit.collider.gameObject.GetComponent<MeshRenderer>().materials);
                    mats.Add(PickupThrow.instance.outlinedMaterial);
                    hit.collider.gameObject.GetComponent<MeshRenderer>().materials = mats.ToArray();
                    obj = hit.collider.gameObject.GetComponent<MeshRenderer>();
                }
            }
            //Item Hide outline
            if(obj)
            {
                if(obj.materials.Length == 2)
                {
                    List<Material> mats = new List<Material>(obj.materials);
                    mats.Remove(PickupThrow.instance.outlinedMaterial);
                    obj.materials = mats.ToArray();
                }
            }
        }

    }
}
