using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public float health = 8;

    private void Start()
    {
        healthBar.GetComponent<Image>();
    }
    
    void Update()
    {
        health = GetComponentInParent<AIScript>().enemyLife;
        healthBar.fillAmount = health / 8;
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
