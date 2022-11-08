using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrickController : MonoBehaviour
{
    public Animator animPatrick;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animPatrick.SetBool("Move", true);
        }

    }
}
