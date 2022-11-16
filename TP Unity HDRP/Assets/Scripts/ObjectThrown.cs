using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThrown : MonoBehaviour
{
    PatrickController papate;

    void Awake() 
    {
        papate = PatrickController.instance;
    }

    void OnCollisionEnter(Collision other) 
    {
        if(other.collider.tag == "Patrick" && papate.hitCooldown == 1 && GetComponent<Rigidbody>().velocity.magnitude > 1f)
        {
            StartCoroutine(papate.PatrickHit());
        }
    }
}
