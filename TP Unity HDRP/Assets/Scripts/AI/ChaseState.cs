using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public AttackState attackState;
    public PatrolState patrolState;
    public bool isInAttackRange = false;

    PatrickController papate;

    void Start() 
    {
        papate = PatrickController.instance;    
    }

    public override State RunCurrentState()
    {
        if(isInAttackRange)
        {
            return attackState;
        }
        else
        {
            if(papate.target)
            {
                if(!papate.chasing) papate.chasing = true;
                papate.agent.SetDestination(papate.target.transform.position);
            }
            else 
            {
                papate.patrolling = true;
                papate.chasing = false;
                return patrolState; //return searching state
            }

            return this;
        }
    }
}
