using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public AttackState attackState;
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
                print("agent");
                papate.agent.SetDestination(papate.target.transform.position);
            }
            else 
            {
                return this; //return searching state
            }

            return this;
        }
    }
}
