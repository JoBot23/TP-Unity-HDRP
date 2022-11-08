using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InputController : MonoBehaviour
{
    public GameObject CMCam;
    private bool canUnlock = false;
    private float zoomChangeAmount = 5f;
    public GameObject roof;
    public Transform roofAI;
    public bool roofEnabled = true;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            canUnlock = true;
            SwitchCam();
        }
        if (Input.GetMouseButtonUp(1))
        {
            canUnlock = false;
            SwitchCam();
        }

        if(Input.mouseScrollDelta.y > 0 && CMCam.GetComponent<CinemachineFreeLook>().m_Orbits[1].m_Height > 7)
        {
            for (int i = 0; i < 3; i++)
                CMCam.GetComponent<CinemachineFreeLook>().m_Orbits[i].m_Height -= (zoomChangeAmount * Time.deltaTime * 20);
        }
        if (Input.mouseScrollDelta.y < 0 && CMCam.GetComponent<CinemachineFreeLook>().m_Orbits[1].m_Height < 16)
        {
            for (int i = 0; i < 3; i++)
                CMCam.GetComponent<CinemachineFreeLook>().m_Orbits[i].m_Height += (zoomChangeAmount * Time.deltaTime * 20);
        }

        if ((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.DownArrow)) && GetComponentInParent<AgentController>().PlayerPosition().y < 99 && PlayerChased() == false)
        {
            roof.SetActive(false);
            roofEnabled = false;
            if (GetComponentInParent<ClicAgentController>().rh.point.y >= 99.2f)
            {
                GetComponentInParent<AgentController>().agent.SetDestination(GetComponentInParent<AgentController>().PlayerPosition());
            }
            HideEnemies();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            roof.SetActive(true);
            roofEnabled = true;
            UnHideEnemies();
        }
    }

    public bool PlayerChased()
    {
        bool retourne = false;
        foreach (Transform enemy in roofAI)
        {
            if(enemy.GetChild(0).GetComponent<AIScript>().patrolling == false && enemy.GetChild(0).position.y > 99)
            {
                retourne = true;
            }
        }
        return retourne;   
    }

    public void HideEnemies()
    {
        foreach (Transform child in roofAI){
            if (child.GetChild(0).GetComponent<AIScript>().patrolling == true)
            {
                child.GetChild(0).GetComponent<AISenseHearing>().enabled = false;
                child.GetChild(0).GetComponent<AISenseSight>().enabled = false;
                child.GetChild(0).GetChild(0).gameObject.SetActive(false);
                child.GetChild(0).GetChild(1).gameObject.SetActive(false);
                child.GetChild(0).Find("Light").gameObject.SetActive(false);
            }
            
        }

    }
    public void UnHideEnemies()
    {
        foreach (Transform child in roofAI)
        {
            child.GetChild(0).GetComponent<AISenseHearing>().enabled = true;
            child.GetChild(0).GetComponent<AISenseSight>().enabled = true;
            child.GetChild(0).GetChild(0).gameObject.SetActive(true);
            child.GetChild(0).GetChild(1).gameObject.SetActive(true);
            child.GetChild(0).Find("Light").gameObject.SetActive(true);
        }
    }

    void SwitchCam()
    {
        if (canUnlock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            CMCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 250;
            
        }
        else
        {
            CMCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 0;
            Cursor.lockState = CursorLockMode.None;
        }   
    }
}
