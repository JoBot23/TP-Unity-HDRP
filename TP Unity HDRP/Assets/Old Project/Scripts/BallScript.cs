using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public bool right = true;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -158f && right == false) right = true;
        if (transform.position.x > -160f && transform.position.x < 160f && right == true) right = true;
        else if(right == true) right = false;

        if(right) transform.Translate(Vector3.right * 19.5f * Time.deltaTime);
        else transform.Translate(Vector3.left * 19.5f * Time.deltaTime);
    }
}
