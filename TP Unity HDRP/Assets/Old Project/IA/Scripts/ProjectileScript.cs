using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public bool canHit = true;
    private void Start()
    {
        StartCoroutine(CanHit());
    }

    IEnumerator CanHit()
    {
        yield return new WaitForSeconds(1.5f);
        canHit = false;
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
