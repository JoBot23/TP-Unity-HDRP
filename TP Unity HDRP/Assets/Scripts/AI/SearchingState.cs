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
        StartCoroutine(WaitSeek());
    }

    public override State RunCurrentState()
    {
        if(papate.canSeePlayer || papate.canHearPlayer) 
        {
            papate.ChasingSight();
            return chaseState;
        }

        if(hasWaited)
        {
            papate.PatrollingSight();
            return patrolState;
        } 
        else
        {
            return this;
        }
    }

    IEnumerator WaitSeek()
    {
        print(hasWaited);
        yield return new WaitForSeconds(5f);
        papate.patrolling = true;
        papate.chasing = false;
        hasWaited = true;
    }
}
