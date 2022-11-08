using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowScript : MonoBehaviour
{

    public Transform PrefabProjectile;
    public float ProjectileStartSpeed = 50;
    public float OffsetForwardShoot = 2;

    private bool canShoot = true;
    private Transform proj;

    public void ThrowObj() 
    {
        if (canShoot)
        {
            //Création du projetctile au bon endroit
            proj = Instantiate(PrefabProjectile, new Vector3(transform.position.x, transform.position.y + 0.65f, transform.position.z) + transform.forward * OffsetForwardShoot, transform.rotation);
            //Ajout d une impulsion de départ
            proj.GetComponent<Rigidbody>().AddForce(transform.forward * ProjectileStartSpeed, ForceMode.Impulse);
            StartCoroutine(CoolDownShoot());
        }
        
    }

    IEnumerator CoolDownShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.6f);
        canShoot = true;
    }
}
