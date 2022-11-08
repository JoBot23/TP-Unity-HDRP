using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public Canvas tutoCanvas;
    public AudioSource musique;
    void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return) && tutoCanvas.enabled == true)
        {
            tutoCanvas.gameObject.SetActive(false);
            musique.enabled = true;
            Time.timeScale = 1;
        }
    }
}
