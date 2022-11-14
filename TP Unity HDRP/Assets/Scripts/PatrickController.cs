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
    public Path path;
    public Animator animPatrick;
    public GameObject target;

    [Header("Hearing")]
    [SerializeField] float hearingRangeRunning;
    [SerializeField] float hearingRange;
    [SerializeField] float hearingRangeCrouch;

    [Header("Booleans")]
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
        if(Vector3.Distance(transform.position, player.transform.position) < hearingRangeRunning && player.isSprinting)
        {
            canHearPlayer = true;
        }
        if(Vector3.Distance(transform.position, player.transform.position) < hearingRange && !player.isCrouching)
        {
            canHearPlayer = true;
        }
        else if(Vector3.Distance(transform.position, player.gameObject.transform.position) < hearingRangeCrouch)
        {
            canHearPlayer = true;
        }
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
