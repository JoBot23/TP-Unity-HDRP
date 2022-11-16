using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingState : State
{
    PatrickController papate;
    public ChaseState chaseState;
    public PatrolState patrolState;
    bool endSeekAnimation;
    bool hasWaited = false;

    void Start() 
    {
        papate = PatrickController.instance;    
    }

    public void StartSeeking()
    {
        hasWaited = false;
        papate.chasing = false;
        papate.animator.SetBool("Move", false);
        papate.animator.SetTrigger("Suspicious");
        papate.agent.isStopped = true;
        StartCoroutine(WaitSeek());
    }

    public override State RunCurrentState()
    {
        if(papate.canSeePlayer || papate.canHearPlayer) 
        {
            papate.agent.isStopped = false;
            papate.ChasingSight();
            chaseState.StartChasing();
            return chaseState;
        }

        if(hasWaited)
        {
            papate.agent.isStopped = false;
            papate.PatrollingSight();
            patrolState.StartPatrol();
            return patrolState;
        } 
        return this;
    }

    IEnumerator WaitSeek()
    {
        yield return new WaitForSeconds(4f);
        if(!papate.chasing && !papate.patrolling)
        {
            hasWaited = true;
            papate.agent.isStopped = false;
        }
    }
}
