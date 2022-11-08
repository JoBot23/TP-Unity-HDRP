using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIScript : MonoBehaviour
{
    public GameObject Player;
    public NavMeshAgent agent;
    public Transform[] waypoints;
    int waypointIndex;
    public Vector3 target;
    public bool patrolling = true;

    public bool canRotate = false;
    private float rotationAmount;
    private float rotationTime;
    private bool searchRotateEnd = false;

    public int enemyLife = 8;
    public float timeToWait = 2f;
    public bool pause = false;
    public bool fuite = false;
    public bool fuited = false;
    public bool getHit = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
        rotationAmount = Random.Range(-50f, -90f);

        canRotate = false;
        enemyLife = 8;
    }

    void Update()
    {
        if (canRotate)
        {
            rotationTime += Time.deltaTime * 0.05f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, rotationAmount, transform.rotation.z)), rotationTime);
            if (rotationTime >= 0.1)
            {
                if (searchRotateEnd) {
                    StartCoroutine(GetBackPatrolling());
                }
                    
                rotationTime = 0;
                rotationAmount = Random.Range(50f, 90f);
                searchRotateEnd = true;
            }
        }

        if (fuited && Vector3.Distance(transform.position, target) < 1)
        {
            patrolling = true;
            fuite = false;
        }
            
        
        if (Vector3.Distance(transform.position, target) < 1 && patrolling && !pause)
        {
            Patrol();
        }

        if(enemyLife <= 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<ClicAgentController>().RemoveCurrentEnemy();
            Destroy(gameObject);
        }

        if (enemyLife <= 2 && !fuited)
        {
            Flee();
        }

        if (enemyLife != 8)
        {
            gameObject.transform.GetChild(4).gameObject.SetActive(true);
        }
    }

    public void Patrol()
    {
        pause = true;
        StartCoroutine(WaitSeconds(timeToWait));
    }

    IEnumerator WaitSeconds(float sec)
    {
        yield return new WaitForSeconds(sec);
        waypointIndex++;
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
        
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
        pause = false;
    }

    IEnumerator GetBackPatrolling()
    {
        yield return new WaitForSeconds(1.5f);
        canRotate = false;
        agent.speed = 2.5f;
        patrolling = true;
        pause = false;

        float dist = 100f;
        for(int i = 0; i < waypoints.Length; i++)
        {
            if (Vector3.Distance(waypoints[i].position, transform.position) < dist){
                dist = Vector3.Distance(waypoints[i].position, transform.position);
                target = waypoints[i].position;
            }
        }
        gameObject.transform.GetChild(4).gameObject.SetActive(false);
        agent.SetDestination(target);
    }

    private void Flee()
    {
        fuite = true;
        canRotate = false;
        agent.speed = 2.5f;
        patrolling = true;
        pause = false;
        fuited = true;

        float dist = 100f;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (Vector3.Distance(waypoints[i].position, transform.position) < dist)
            {
                dist = Vector3.Distance(waypoints[i].position, transform.position);
                target = waypoints[i].position;
            }
        }
        gameObject.transform.Find("Canvas").gameObject.SetActive(false);
        agent.SetDestination(target);
    }

    public void Spotted()
    {
        if (patrolling && gameObject.transform.Find("Spotted").gameObject.active == false)
        {
            gameObject.transform.Find("Spotted").gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("Player").GetComponent<ClicAgentController>().PlaySpottedSound();
            StartCoroutine(SpottedDuration());
        }
    }

    IEnumerator SpottedDuration()
    {
        yield return new WaitForSeconds(1f);
        gameObject.transform.Find("Spotted").gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            if (other.gameObject.GetComponent<ProjectileScript>().canHit)
            {
                enemyLife--;
                other.gameObject.GetComponent<ProjectileScript>().DestroyProjectile();
                transform.Translate(Vector3.back / 2);
                getHit = true;
            }
        }
    }
}
