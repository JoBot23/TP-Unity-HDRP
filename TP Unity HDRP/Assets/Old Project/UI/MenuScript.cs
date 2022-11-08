using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public PlayerController player;
    public bool pauseToggle = false;
    public GameObject canvasV;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {   
            if (pauseToggle)
            {
                Cursor.lockState = CursorLockMode.Locked;
                player.enableCam();
                canvasV.SetActive(false);
                pauseToggle = !pauseToggle;
                player.canMove = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                player.disableCam();
                pauseToggle = !pauseToggle;
                canvasV.SetActive(true);
                player.canMove = false;
            }
        }
    }
}
