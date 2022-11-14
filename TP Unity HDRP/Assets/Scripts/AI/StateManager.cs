using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateManager : MonoBehaviour
{

    public State currentState;
    PatrickController papate;

    private void Start() 
    {
        papate = PatrickController.instance;
    }

    void Update()
    {
        if(!papate.canSeePlayer && PlayerDetected())
        {
            papate.canSeePlayer = true;
            papate.target = PlayerDetected();
        }
        else if(papate.canSeePlayer && !PlayerDetected())
        {
            papate.canSeePlayer = false;
            papate.target = null;
        }
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState();

        if(nextState != null)
        {
            SwitchToNextState(nextState);
        }
    }

    private void SwitchToNextState(State _nextState)
    {
        currentState = _nextState;
    }

    private GameObject PlayerDetected()
    {
        if(papate.sightSense.objects.Count > 0)
            return papate.sightSense.objects[0];
        return null;
    }
}
