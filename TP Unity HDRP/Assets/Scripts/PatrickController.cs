using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrickController : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent agent;
    public Path path;

    public Animator animPatrick;

    void Start() 
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {


    }
}
