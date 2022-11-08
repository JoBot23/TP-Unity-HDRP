using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    public Transform[] PrefabProjectile;
    public float ProjectileStartSpeed = 50;
    public float OffsetForwardShoot = 2;
    public float MomentToThrow = 0.33f;

    public bool enableThrow;
    public bool canThrow = true;

    void Update()
    {
        if (Random.Range(1, 100) < 25 && canThrow == true && enableThrow)
        {
            GetComponent<Animator>().SetTrigger("Throw");
            canThrow = false;
            StartCoroutine(cooldownThrow());
        }
    }

    IEnumerator cooldownThrow()
    {
        yield return new WaitForSeconds(2f);
        canThrow = true;
    }

    public void ThrowTheObject(){
        //Création du projetctile au bon endroit
        Transform proj = GameObject.Instantiate<Transform>(PrefabProjectile[Random.Range(0,7)], new Vector3(transform.position.x-0.4f, transform.position.y+3.7f, transform.position.z) + transform.forward * OffsetForwardShoot, transform.rotation);
        //Ajout d une impulsion de départ
        proj.GetComponent<Rigidbody>().AddForce(transform.forward * ProjectileStartSpeed, ForceMode.Impulse);
    }
}
