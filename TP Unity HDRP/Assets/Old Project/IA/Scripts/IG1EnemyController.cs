using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class IG1EnemyController : AgentController
{
    private NavMeshAgent agentEnemy;

    void Start()
    {
        agentEnemy = GetComponent<NavMeshAgent>();
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<AISenseSight>().AddSenseHandler(new AISense<SightStimulus>.SenseEventHandler(HandleSight));
        GetComponent<AISenseSight>().AddObjectToTrack(player);
        GetComponent<AISenseHearing>().AddSenseHandler(new AISense<HearingStimulus>.SenseEventHandler(HandleHearing));
        GetComponent<AISenseHearing>().AddObjectToTrack(player);
    }

    void HandleSight(SightStimulus sti, AISense<SightStimulus>.Status evt)
    {
        if (evt == AISense<SightStimulus>.Status.Enter)
            Debug.Log("Objet " + evt + " vue en " + sti.position);
        GetComponent<AIScript>().patrolling = false;
        agentEnemy.speed = 4f;
        FindPathTo(sti.position);


        if ((sti.position - transform.position).sqrMagnitude < 1 * 1)
        {
            //GameObject.Find("Door Deco").GetComponent<DoorScript>().PlayDefeatSound();
            GameObject.Find("Door Deco").GetComponent<DoorScript>().loseCanvas.gameObject.SetActive(true);
        }
    }

    void HandleHearing(HearingStimulus sti, AISense<HearingStimulus>.Status evt)
    {
        if (evt == AISense<HearingStimulus>.Status.Enter)
            Debug.Log("Objet " + evt + " ouïe en " + sti.position);
        GetComponent<AIScript>().patrolling = false;
        agentEnemy.speed = 4f;
        FindPathTo(sti.position);
    }
}
