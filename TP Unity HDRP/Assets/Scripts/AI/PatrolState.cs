using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    [SerializeField] ChaseState chaseState;
    PatrickController papate;
    public int waypointIndex;
    public float waitBetweenWaypoints;
    private float waitTimer;

    void Start() 
    {
        papate = PatrickController.instance;    
    }

    public override State RunCurrentState()
    {
        if(papate.canSeePlayer || papate.canHearPlayer) 
        {
            papate.patrolling = false;
            return chaseState;
        }
        else
        {
            Patrol();
            return this;
        }
    }

    private void Patrol()
    {
        if(PatrickController.instance.agent.remainingDistance < 0.5f)
        {
            waitTimer += Time.deltaTime;
            if(waitTimer > waitBetweenWaypoints)
            {
                if(waypointIndex < PatrickController.instance.path.waypoints.Count-1)
                {
                    waypointIndex++;
                }
                else waypointIndex = 0;
                PatrickController.instance.agent.SetDestination(PatrickController.instance.path.waypoints[waypointIndex].position);
                waitTimer = 0;
            }
        }
    }
}
