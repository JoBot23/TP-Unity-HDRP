using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    public int waypointIndex;
    public float waitBetweenWaypoints;
    private float waitTimer;
    PatrickController papate;
    void Start() 
    {
        papate = transform.parent.parent.GetComponent<PatrickController>();
    }

    public override State RunCurrentState()
    {
        Patrol();
        return this;
    }

    private void Patrol()
    {
        if(papate.agent.remainingDistance < 0.5f)
        {
            waitTimer += Time.deltaTime;
            if(waitTimer > waitBetweenWaypoints)
            {
                if(waypointIndex < papate.path.waypoints.Count-1)
                {
                    waypointIndex++;
                }
                else waypointIndex = 0;
                papate.agent.SetDestination(papate.path.waypoints[waypointIndex].position);
                waitTimer = 0;
            }
        }
    }
}
