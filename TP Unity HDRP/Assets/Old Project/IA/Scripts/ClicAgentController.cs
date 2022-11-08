using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClicAgentController : AgentController
{
    private Ray rayPickPos; //Déclaré ici pour pouvoir le visualiser, il doit rester accessible entre deux clics
    public RaycastHit rh;
    public float baseSpeed = 3.5f;
    private float lastClickTime;
    public Transform objectif;
    public DoorScript doorScript;

    public List<GameObject> ennemys;
    public GameObject targetEnnemy;

    public AudioClip spottedClip;

    private void Start()
    {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            ennemys.Add(enemy);
        }
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //Deplacement
        if (Input.GetButtonDown("Fire1"))
        {
            rayPickPos = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(rayPickPos, out rh))
            {
                if (rh.point.y > 98f && GetComponentInChildren<InputController>().roofEnabled == false)
                    canGetThere = false;
                else
                    canGetThere = true;

                if (ennemys.Count >= 1 && Vector3.Distance(GetClosestEnnemy().transform.position, rh.point) < 1f && Vector3.Distance(GetClosestEnnemy().transform.position, transform.position) < 9)
                {
                    foreach (GameObject obj in ennemys){
                        if(obj != GetClosestEnnemy())
                            obj.transform.GetChild(2).gameObject.SetActive(false);
                    }

                    if(GetClosestEnnemy().transform.GetChild(2).gameObject.active == false){
                        GetClosestEnnemy().transform.GetChild(2).gameObject.SetActive(true);
                        GetClosestEnnemy().transform.GetChild(4).gameObject.SetActive(true);
                    }
                    Vector3 directionToFace = GetClosestEnnemy().transform.position - transform.position;
                    transform.rotation = Quaternion.LookRotation(directionToFace);
                    gameObject.transform.GetComponentInChildren<ThrowScript>().ThrowObj();
                }
                else if(!doorScript.haveKey && Vector3.Distance(objectif.position, rh.point) < 2f)
                {
                    objectif.gameObject.SetActive(true);
                    FindPathTo(rh.point);
                }
                else
                {
                    FindPathTo(rh.point);
                    foreach (GameObject obj in ennemys)
                    {
                        obj.transform.GetChild(2).gameObject.SetActive(false);
                        GetClosestEnnemy().transform.GetChild(4).gameObject.SetActive(false);
                    }
                }
            }
            
            float timeSinceLastClick = Time.time - lastClickTime;
            if(timeSinceLastClick <= 0.2f)
            {
                if (agent.velocity.sqrMagnitude > 0.0f)
                {
                    agent.speed = baseSpeed * 1.5f;
                    GetComponent<NoiseStatus>().NoiseLevel = 1;
                }
                else
                {
                    agent.speed = baseSpeed;
                    GetComponent<NoiseStatus>().NoiseLevel = 0;
                }
            }
            else
            {
                agent.speed = baseSpeed;
                GetComponent<NoiseStatus>().NoiseLevel = 0;
            }

            lastClickTime = Time.time;
        }
    }

    public GameObject GetClosestEnnemy()
    {
        float dist = 100f;
        for (int i = 0; i < ennemys.Count; i++)
        {
            if (Vector3.Distance(ennemys[i].transform.position, rh.point) < dist)
            {
                dist = Vector3.Distance(ennemys[i].transform.position, rh.point);
                targetEnnemy = ennemys[i];
            }
        }
        return targetEnnemy;
    }

    public void RemoveCurrentEnemy()
    {
        foreach(GameObject enemy in ennemys)
        {
            if (enemy.transform.GetChild(2).gameObject.active)
            {
                ennemys.Remove(enemy);
                break;
            }
        }
    }

    public void PlaySpottedSound()
    {
        GetComponent<AudioSource>().PlayOneShot(spottedClip);
    }

    public new void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(rayPickPos.origin, rayPickPos.direction * 100);
    }
}