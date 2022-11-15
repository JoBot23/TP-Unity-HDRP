using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrickController : MonoBehaviour
{
    public static PatrickController instance;
    [SerializeField] PlayerController player;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public AISight sightSense;
    public List<Path> patrolPaths = new List<Path>();
    public Path path;
    public Animator animPatrick;
    public GameObject target;

    [Header("Parameters")]
    public float patrollingSpeed;
    public float chasingSpeed;
    public float throughPatrolSpeed;

    [Header("Hearing")]
    [SerializeField] float hearingRangeRunning;
    [SerializeField] float hearingRange;
    [SerializeField] float hearingRangeCrouch;

    [Header("Booleans")]
    public bool chasing;
    public bool canSeePlayer;
    public bool canHearPlayer;
    public bool patrolling = true;

    void Awake() 
    {
        instance = this;
        agent = GetComponent<NavMeshAgent>();
        sightSense = GetComponent<AISight>();
    }

    void Update()
    {
        //Speed
        if(patrolling) agent.speed = patrollingSpeed;
        else if(chasing) agent.speed = chasingSpeed;
        else agent.speed = throughPatrolSpeed;

        //Sight
        if(!canSeePlayer && PlayerDetected())
        {
            canSeePlayer = true;
            target = PlayerDetected();
        }
        else if(canSeePlayer && !PlayerDetected())
        {
            canSeePlayer = false;
            target = null;
        }

        //Hearing
        if(Vector3.Distance(transform.position, player.transform.position) < hearingRangeRunning && player.isSprinting)
        {
            canHearPlayer = true;
        }
        else if(Vector3.Distance(transform.position, player.transform.position) < hearingRange && !player.isCrouching)
        {
            canHearPlayer = true;
        }
        else if(Vector3.Distance(transform.position, player.gameObject.transform.position) < hearingRangeCrouch)
        {
            canHearPlayer = true;
        }
        else canHearPlayer = false;
    }

    public void PatrollingSight()
    {
        sightSense.sightAngle = 70;
        sightSense.sightRange = 14;
        sightSense.OnValidate();
    }

    public void ChasingSight()
    {
        sightSense.sightAngle = 90;
        sightSense.sightRange = 16;
        sightSense.OnValidate();
    }

    private GameObject PlayerDetected()
    {
        if(sightSense.objects.Count > 0)
            return sightSense.objects[0];
        if(canHearPlayer)
            return player.gameObject;
        
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, hearingRangeRunning);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hearingRange);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, hearingRangeCrouch);
    }
}
