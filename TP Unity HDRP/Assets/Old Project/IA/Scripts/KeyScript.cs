using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public DoorScript doorScript;
    public GameObject pillar;
    public GameObject crossCircle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.Find("Door Deco").GetComponent<DoorScript>().PlayKeyPickupSound();
            doorScript.haveKey = true;
            Destroy(pillar.gameObject);
            Destroy(crossCircle.gameObject);
            Destroy(gameObject);
        }
    }
}
