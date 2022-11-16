using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public AttackState attackState;
    public SearchingState seekingState;
    public bool isInAttackRange = false;
    private Vector3 lastSeen;

    PatrickController papate;

    void Start() 
    {
        papate = PatrickController.instance;    
    }

    public void StartChasing()
    {
        papate.patrolling = false;
        papate.chasing = true;
        papate.agent.speed = papate.chasingSpeed;
        papate.animator.SetBool("Move", true);
        papate.animator.SetFloat("Speed", papate.agent.speed);
    }

    public override State RunCurrentState()
    {
        
        if(papate.target)
        {
            lastSeen = papate.target.transform.position;
        }
        else if(papate.agent.remainingDistance < 0.5f)
        {
            seekingState.StartSeeking();
            return seekingState;
        }

        if(lastSeen != null) papate.agent.SetDestination(lastSeen);
        return this;
    }
}
