using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    [SerializeField] ChaseState chaseState;
    PatrickController papate;
    public int waypointIndex;
    public float waitWaypointTime;
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
            papate.ChasingSight();
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
            if(waypointIndex < PatrickController.instance.path.waypoints.Count-1)
            {
                if(PatrickController.instance.path.waypoints[waypointIndex].tag == "WaitWaypoint")
                {
                    waitTimer += Time.deltaTime;
                    if(waitTimer > waitWaypointTime)
                    {
                        waypointIndex++;
                        waitTimer = 0;
                        PatrickController.instance.agent.SetDestination(PatrickController.instance.path.waypoints[waypointIndex].position);
                        if(waypointIndex == PatrickController.instance.path.waypoints.Count-1) waypointIndex = 0;
                    }
                }
                else
                {
                    waypointIndex++;
                    PatrickController.instance.agent.SetDestination(PatrickController.instance.path.waypoints[waypointIndex].position);
                }
            }
            else 
            {
                waypointIndex = 0; 
            }
           
        }
    }
}
