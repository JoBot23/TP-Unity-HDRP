using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool canMove = true;
    public bool canCrouch = true;
    public bool canSprint = true;
    public bool isSprinting;

    public CharacterController controller;
    public Camera cam;
    public float speed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float jumpHeight;

    public float gravity = -9.81f;
    private Vector3 velocity;

    private bool grounded;
    public Transform groundCheck;
    public LayerMask groundMask;


    [Header("Crouch Parameters")]
    [SerializeField] float crouchHeight;
    [SerializeField] float standingHeight;
    [SerializeField] float timeToCrouch;
    [SerializeField] Vector3 crouchingCenter = new Vector3(0,0.5f,0);
    [SerializeField] Vector3 standingCenter = new Vector3(0,0,0);
    bool isCrouching;
    bool crouchAnimation;

    [Header("Head Bobbing")]
    private bool canUseHeadBob = true;
    [SerializeField] private float walkBobSpeed;
    [SerializeField] private float walkBobAmount;
    [SerializeField] private float sprintBobSpeed;
    [SerializeField] private float sprintBobAmount;
    [SerializeField] private float crouchBobSpeed;
    [SerializeField] private float crouchBobAmount;
    private float defaultYPos;
    private float timer;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        defaultYPos = cam.transform.localPosition.y;
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
            if(isSprinting) controller.Move(move * sprintSpeed * Time.deltaTime);
            else controller.Move(move * (isCrouching ? crouchSpeed :  speed) * Time.deltaTime);
            

            // Gravit�
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            //Sprint
            if(canSprint)
            {
                Sprint();
            }

            //Crouch
            if(canCrouch)
            {
                Crouch();
            }

            //Head Bobbing
            if(canUseHeadBob)
            {
                HeadBob();
            }
        }
        
    }

    private void Sprint()
    {
        if(Input.GetKey(KeyCode.LeftShift) && controller.isGrounded && !crouchAnimation && !isCrouching)
        {
            //if(isCrouching) StartCoroutine(CrouchStand());
            isSprinting = true;
        }
        else isSprinting = false;
    }

    private void Crouch()
    {
        if(Input.GetKeyDown(KeyCode.C) && !crouchAnimation && controller.isGrounded)
        {
            StartCoroutine(CrouchStand());
        }
    }

    private IEnumerator CrouchStand()
    {
        if(isCrouching && Physics.Raycast(cam.transform.position, Vector3.up, 1f))
            yield break;
        if(isSprinting) isSprinting = false;

        crouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = controller.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = controller.center;

        while(timeElapsed < timeToCrouch)
        {
            controller.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            controller.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }


        controller.height = targetHeight;
        controller.center = targetCenter;

        isCrouching = !isCrouching;

        crouchAnimation = false;
    }

    private void HeadBob()
    {
        if(!controller.isGrounded) return;

        if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : isSprinting ? sprintBobSpeed : walkBobSpeed);
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : isSprinting ? sprintBobAmount : walkBobAmount), cam.transform.localPosition.z);
        }

    }
}
