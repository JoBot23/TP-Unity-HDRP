using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool canMove = true;

    public CharacterController controller;
    public float speed = 8f;
    public float jumpHeight = 200f;

    public float gravity = -9.81f;
    Vector3 velocity;

    public bool grounded;
    public Transform groundCheck;
    public LayerMask groundMask;

    public Transform PrefabProjectile;
    public float ProjectileStartSpeed = 50;
    public float OffsetForwardShoot = 2;
    public float TimeBetweenShots = 0.5f;
    private float TimeShoot = 0;
    public bool WantsToShoot;

    public Material[] balls;

    //Cam
    public bool camSwitch;
    public GameObject camFPS;
    
    public GameObject camTPS;
    public GameObject camCine;

    // Update is called once per frame

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (canMove)
        {
            grounded = Physics.CheckSphere(groundCheck.position, 0.6f, groundMask);

            if (grounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if (Input.GetButtonDown("Jump") && grounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);

            // Gravit�
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            // Gun
            TimeShoot -= Time.deltaTime;
            if (Input.GetButton("Fire1") && TimeShoot <= 0)
            {
                TimeShoot = TimeBetweenShots;

                //Cr�ation du projetctile au bon endroit

                PrefabProjectile.GetComponent<Renderer>().material = balls[Random.Range(0, 5)];
                Transform proj = GameObject.Instantiate<Transform>(PrefabProjectile, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * OffsetForwardShoot, transform.rotation);
                //Ajout d une impulsion de d�part
                proj.GetComponent<Rigidbody>().AddForce(transform.forward * ProjectileStartSpeed, ForceMode.Impulse);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                if (!camSwitch)
                {
                    camFPS.SetActive(false);
                    camTPS.SetActive(true);
                    camCine.SetActive(true);
                }
                else if(camSwitch)
                {
                    camFPS.SetActive(true);
                    camTPS.SetActive(false);
                    camCine.SetActive(false);
                }
                camSwitch = !camSwitch;
            }
        }
        
    }

    public void disableCam()
    {
        if (camSwitch)
            camCine.SetActive(false);
    }

    public void enableCam()
    {
        if(camSwitch)
            camCine.SetActive(true);
    }
}
