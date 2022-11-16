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

    public void StartPatrol()
    {
        papate.patrolling = true;
        papate.agent.speed = papate.patrollingSpeed;
        papate.animator.SetBool("Move", true);
        papate.animator.SetFloat("Speed", papate.agent.speed);
        papate.chaseMusic.enabled = false;
    }

    public override State RunCurrentState()
    {
        if(papate.canSeePlayer || papate.canHearPlayer) 
        {
            papate.ChasingSight();
            chaseState.StartChasing();
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
        if(papate.agent.remainingDistance < 0.5f)
        {
            if(waypointIndex < papate.path.waypoints.Count-1)
            {
                if(papate.path.waypoints[waypointIndex].tag == "WaitWaypoint") //Timed waypoint
                {
                    //papate.animator.SetBool("Move", false);
                    waitTimer += Time.deltaTime;
                    if(waitTimer > waitWaypointTime)
                    {
                        waypointIndex++;
                        papate.agent.SetDestination(papate.path.waypoints[waypointIndex].position);
                        papate.animator.SetBool("Move", true);
                        waitTimer = 0;
                    }
                }
                else //Simple waypoint
                {
                    waypointIndex++;
                    papate.agent.SetDestination(papate.path.waypoints[waypointIndex].position);
                }
            }
            else 
            {
                papate.ChangePatrolPath();
                papate.PatrickWhistle();
                waypointIndex = 0; 
            }
           
        }
    }
}
