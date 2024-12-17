using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBCore;
using KBCore.Refs;
using Cinemachine;
using static UnityEngine.Rendering.DebugUI;
using Utilities;

namespace Platformer
{
    public class PlayerController : ValidatedMonoBehaviour
    {
        [Header("References")]

        
        [SerializeField, Self] Animator animator;
        [SerializeField ,Self] Rigidbody rb;
        [SerializeField,Self] GroundChecker groundChecker;
        [SerializeField, Anywhere] CinemachineFreeLook freeLookVcam;
        [SerializeField, Anywhere] InputReader input;




        [Header("Movement Settings")]
        [SerializeField] float movSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = 0.2f;


        [Header("Jump Settings")]
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float jumpDuration = 0.5f;
        [SerializeField] float jumpCooldown = 0f;
        [SerializeField] float jumpMaxHieght = 1.5f;
        [SerializeField] float gravityMultiplier = 3f; 

        Transform mainCam;

        const float ZeroF = 0f;

        float currentSpeed;
        float velocity;
        float jumpVelocity;

        Vector3 movement;

        List<Timer> timers;
        CountdownTimer jumpTimer;
        CountdownTimer jumpCooldownTimer;


        //Animator parameters

        static readonly int Speed = Animator.StringToHash("Speed");


        void Awake()
        {   
            mainCam = Camera.main.transform;
            freeLookVcam.Follow = transform;
            freeLookVcam.LookAt = transform;
            //Invoke event when observed  transform is teleported , adjusting freeLookVcam's position accordingly
            freeLookVcam.OnTargetObjectWarped(transform, transform.position - freeLookVcam.transform.position - Vector3.forward);

            rb.freezeRotation = true;

            // Setup timers

            jumpTimer = new CountdownTimer(jumpDuration);
            jumpCooldownTimer = new CountdownTimer(jumpCooldown);
            timers  = new List<Timer>(capacity:2) {jumpTimer,jumpCooldownTimer };

            // cooldown evrytime we finish jumping
            jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();
        }

        
        void Start()
        {
            input.EnablePlayerActions();   
        }

        void OnEnable()
        {
            input.Jump += OnJump;    
        }
        void OnDisable()
        {
            input.Jump -= OnJump;
        }

        void OnJump(bool performed)
        {
            if (performed && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpTimer.Start();
                
            }
            else if (!performed && jumpTimer.IsRunning) 
            {
                jumpTimer.Stop();
            }
        }
        void Update()
        {   
            movement = new Vector3(input.Direction.x,0f,input.Direction.y);
             
            UpdateAnimator();
            HandleTimers();
        }
        void FixedUpdate()
        {   
            HandleJump();
            HandleMovement();
        }

        void UpdateAnimator()
        {
            animator.SetFloat(Speed, currentSpeed);
        }

        void HandleTimers()
        {
            foreach (var timer in timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }

        void HandleJump()
        {
            if(!jumpTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpVelocity = ZeroF;
                jumpTimer.Stop();
                return;
            }

            // If jumping or falling calculate velocity
            if (jumpTimer.IsRunning)
            {   
                // Progress point for intial burst of velocity 
                float launchPoint = 0.1f;
                if (jumpTimer.Progress > launchPoint)
                {   
                    // calculate the velocity required to reach the jump height using Physics  equation  v = sqrt(2gh)
                    jumpVelocity = Mathf.Sqrt(2 * jumpMaxHieght * Mathf.Abs(Physics.gravity.y));
                }
                else
                {
                    //Gradually apply less velocity as the jump progress 
                    jumpVelocity += (1 - jumpTimer.Progress) * jumpForce * Time.fixedDeltaTime;
                }
            }
            else
            {
                // Gravity takes over 
                jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
            }

            // Apply velocity
            rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);

        }
        void HandleMovement()
        {
            Vector3 movementDirection = new Vector3(input.Direction.x,0f,input.Direction.y).normalized;
            Vector3 adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y,Vector3.up) * movement;

            if(adjustedDirection.magnitude > ZeroF)
            {

                HandleRotation(adjustedDirection);
                HandleHorizontalMovement(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
                SmoothSpeed(ZeroF);

                // Reset the horizontal velocity for a snappy stop

                rb.velocity =  new Vector3(ZeroF,rb.velocity.y,ZeroF);
            }
        }
       
        void HandleRotation(Vector3 adjustedDirection)
        {   
            //Adjust rotation to match direction 
            Quaternion targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.LookAt(transform.position + adjustedDirection);

        } 
        void HandleHorizontalMovement(Vector3 adjustedDirection)
        {
            // Move Player
            Vector3 velocity = adjustedDirection * movSpeed * Time.fixedDeltaTime;
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        }

        void SmoothSpeed(float value)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
        }

    }
}
