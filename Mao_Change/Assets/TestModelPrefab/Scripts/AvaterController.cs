using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AvaterController  {
    private Animator m_anim;
    private GameObject Model;
    public GameObject Player;

    private enum CharacterState
    {
        IDLE = 0,
        WALKING = 1,
        TROTING = 2,
        RUNNING = 3,
        JUMPING = 4,

    }

    private CharacterState m_characterState;

    float walkSpeed = 2.0f;
    float trotSpeed = 4.0f;
    float runSpeed = 20.0f;//6.0

    float inAirControlAcceleration = 3.0f;

    float jumpHeight = 3.0f;

    float gravity = 20;
    // The gravity in controlled descent mode
    float speedSmoothing = 10.0f;
    float rotateSpeed = 500.0f;
    float trotAfterSeconds = 3.0f;

    bool canJump = true;

    private float jumpRepeatTime = 0.05f;
    private float jumpTimeout = 0.15f;
    private float groundedTimeout = 0.25f;

    private float lockCameraTimer = 0.0f;

    // The current move direction in x-z
    private Vector3 moveDirection = Vector3.zero;
    // The current vertical speed
    private float verticalSpeed = 0.0f;
    // The current x-z move speed
    private float moveSpeed = 0.0f;

    // The last collision flags returned from controller.Move
    private CollisionFlags collisionFlags;

    // Are we jumping? (Initiated with jump button and not grounded yet)
    private bool jumping = false;
    private bool jumpingReachedApex = false;

    // Are we moving backwards (This locks the camera to not do a 180 degree spin)
    private bool movingBack = false;
    // Is the user pressing any keys?
    private bool isMoving = false;
    // When did the user start walking (Used for going into trot after a while)
    private float walkTimeStart = 0.0f;
    // Last time the jump button was clicked down
    private float lastJumpButtonTime = -10.0f;
    // Last time we performed a jump
    private float lastJumpTime = -1.0f;

    // the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)
    private float lastJumpStartHeight = 0.0f;

    private Vector3 inAirVelocity = Vector3.zero;

    private float lastGroundedTime = 0.0f;

    private bool isControllable = true;

    private float CheckFlyTime = 0;

    //private GameObject TheLastFloor;//fly use ground check
    private float FlyHeight = 3f;

    //fly and ground
    float flytime = 0;
    float maxFlytime = 20;
    private AudioController audioController;
    private enum MoveState
    {
        Null = -1,
        Ground = 0,
        Fly = 1,
    };
    private MoveState m_moveState = MoveState.Null;

    public void Initialized()
    {
        moveDirection = Player.transform.TransformDirection(Vector3.forward);
        m_anim = Player.transform.GetChild(0).GetComponent<Animator>();
        //m_anim = Player.GetComponent<Animator>();
        audioController = Player.GetComponent<AudioController>();
    }

   private bool CanMove()
    {
        if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || m_anim.GetCurrentAnimatorStateInfo(0).IsName("Run") || m_anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            return true;
        }
        else
            return false;
    }

    void UpdateSmoothedMovementDirection()
    {
        Transform cameraTransform = Camera.main.transform;
        bool grounded = IsGrounded();

        // Forward vector relative to the camera along the x-z plane	
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        // Right vector relative to the camera
        // Always orthogonal to the forward vector
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        if (v < -0.2f)
            movingBack = true;
        else
            movingBack = false;

        bool wasMoving = isMoving;
        //isMoving = Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;
        isMoving = Mathf.Abs(h) > 0.8f || Mathf.Abs(v) > 0.8f;

        // Target direction relative to the camera
        //Vector3 targetDirection = h * right + v * forward;
        Vector3 targetDirection;
        if(isMoving)
        {
            targetDirection = h * right + v * forward;
        }
        else
        {
            targetDirection = h * right/10 + v * forward/10;
        }

        // Grounded controls
        if (grounded)
        {
            // Lock camera for short period when transitioning moving & standing still
            lockCameraTimer += Time.deltaTime;
            if (isMoving != wasMoving)
                lockCameraTimer = 0.0f;

            // We store speed and direction seperately,
            // so that when the character stands still we still have a valid forward direction
            // moveDirection is always normalized, and we only update it if there is user input.
            if (targetDirection != Vector3.zero)
            {
                // If we are really slow, just snap to the target direction
                if (moveSpeed < walkSpeed * 0.9f && grounded)
                {
                    moveDirection = targetDirection.normalized;
                }
                // Otherwise smoothly turn towards it
                else
                {
                    moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
                    
                    moveDirection = moveDirection.normalized;
                }
            }

            // Smooth the speed based on the current target direction
            float curSmooth = speedSmoothing * Time.deltaTime;

            // Choose target speed
            //* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
            float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);

            m_characterState = CharacterState.IDLE;

            // Pick speed modifier
            if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift) | isMoving)
            {
                targetSpeed *= runSpeed;
                m_characterState = CharacterState.RUNNING;
                m_anim.SetBool("Run", true);
            }
            else if (Time.time - trotAfterSeconds > walkTimeStart)
            {
                targetSpeed *= trotSpeed;
                m_characterState = CharacterState.TROTING;
            }
            else
            {
                targetSpeed *= walkSpeed;
                m_characterState = CharacterState.WALKING;
                m_anim.SetBool("Run", false);
            }

            moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);

            // Reset walk time start when we slow down
            if (moveSpeed < walkSpeed * 0.3f)
                walkTimeStart = Time.time;

        }

        // In air controls
        else
        {
            // Lock camera while in air
            if (jumping)
                lockCameraTimer = 0.0f;

            if (isMoving)
            {
                inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
            }

        }

    }

    void ApplyJumping()
    {
        // Prevent jumping too fast after each other
        if (lastJumpTime + jumpRepeatTime > Time.time)
            return;

        if (IsGrounded())
        {
            // Jump
            // - Only when pressing the button down
            // - With a timeout so you can press the button slightly before landing		
            if (canJump && Time.time < lastJumpButtonTime + jumpTimeout)
            {
                verticalSpeed = CalculateJumpVerticalSpeed(jumpHeight);
                //SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void ApplyGravity()
    {
        if (isControllable) // don't move player at all if not controllable.
        {
            // Apply gravity
            bool jumpButton = Input.GetButton("Jump");

            // When we reach the apex of the jump we send out a message
            if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0f)
            {
                jumpingReachedApex = true;
                //SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
            }

            if (IsGrounded())
                verticalSpeed = 0.0f;
            else
                verticalSpeed -= gravity * Time.deltaTime;
        }
    }

    float CalculateJumpVerticalSpeed(float targetJumpHeight)
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * targetJumpHeight * gravity);
    }

    void DidJump()
    {
        jumping = true;
        jumpingReachedApex = false;
        lastJumpTime = Time.time;
        //lastJumpStartHeight = transform.position.y;
        lastJumpButtonTime = -10;

        m_anim.SetBool("Jump", jumping);

        m_characterState = CharacterState.JUMPING;
    }

    public static float SampleHeight(Vector3 point)
    {

        var sample = point;
        sample.y += 20;

        Ray ray = new Ray(sample, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag != "Terrain")
            {
                Debug.DrawLine(sample, hit.point);
                return hit.point.y;
            }
        }
        return -1;
    }

    
    public bool CheckGround()
    {
        Ray ray = new Ray(Player.transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform != null)
            {
                Debug.DrawLine(Player.transform.position, hit.point);
                return true;
            }
        }
        return false;
    }

    private void ReturnBody()
    {
        moveSpeed = 0;
        m_characterState = CharacterState.IDLE;
        Player.transform.position = new Vector3(0, 20, 0);

    }

    public void Update()
    {
        if(CanMove())
        {
            GroundWalk();
        }
        
    }

    //407
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.moveDirection.y > 0.01f)
            return;
    }

    float GetSpeed()
    {
        return moveSpeed;
    }

    public bool IsJumping()
    {
        return jumping;
    }

    bool IsGrounded()
    {
        return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

    Vector3 GetDirection()
    {
        return moveDirection;
    }

    public bool IsMovingBackwards()
    {
        return movingBack;
    }

    public float GetLockCameraTimer()
    {
        return lockCameraTimer;
    }

    bool IsMoving()
    {
        return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
    }

    bool HasJumpReachedApex()
    {
        return jumpingReachedApex;
    }

    bool IsGroundedWithTimeOut()
    {
        return lastGroundedTime + groundedTimeout > Time.time;
    }

    void Reset()
    {
        Player.tag = "Player";
    }

    void GroundWalk()
    {
        if (!CheckGround())
        {
            CheckFlyTime += Time.deltaTime;
            if (CheckFlyTime > 2.0f)
                ReturnBody();
        }
        else
        {
            CheckFlyTime = 0;
        }

        if (!isControllable)
        {
            // kill all inputs if not controllable.
            Input.ResetInputAxes();
        }

        if (Input.GetButtonDown("Jump"))
        {
            lastJumpButtonTime = Time.time;
        }

        UpdateSmoothedMovementDirection();

        // Apply gravity
        // - extra power jump modifies gravity
        // - controlledDescent mode modifies gravity
        ApplyGravity();//-------

        // Apply jumping logic
        //ApplyJumping();//--------

        if (IsMoving())
        {
            //var newPos = Player.transform.position + (Player.transform.rotation * Vector3.forward * moveSpeed);
            //-------------------1231-----------
            ////newPos.y = Terrain.activeTerrain.SampleHeight(newPos);
            //newPos.y = SampleHeight(newPos);

            //var heropos = transform.position;
            ////heropos.y = Terrain.activeTerrain.SampleHeight(transform.position);
            //heropos.y = SampleHeight(transform.position);
            //Debug.DrawLine(heropos, newPos, Color.red);
            //-----------------1231------------------

            //var c = moveSpeed;
            //----------1231--------------
            //var b = newPos.y - heropos.y;
            //if (b > 0)
            //{
            //    var a = Mathf.Sqrt(Mathf.Pow(c, 2) - Mathf.Pow(b, 2));
            //    moveSpeed = a;
            //}
            //----------1231--------------
            
        }
        

        if(m_anim.GetCurrentAnimatorStateInfo(0).IsName("Walk")|| m_anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            audioController.PlaySound(0);
        }
        else
        {
            audioController.StopSound(0);
        }

        // Calculate actual motion
        Vector3 movement = moveDirection * moveSpeed + new Vector3(0, verticalSpeed, 0) + inAirVelocity;
        movement *= Time.deltaTime;

        // Move the controller
        CharacterController controller = Player.GetComponent<CharacterController>();
        collisionFlags = controller.Move(movement);

        // Set rotation to the move direction
        if (IsGrounded())
        {

            Player.transform.rotation = Quaternion.LookRotation(moveDirection);

        }
        else
        {
            Vector3 xzMove = movement;
            xzMove.y = 0;
            if (xzMove.sqrMagnitude > 0.001f)
            {
                Player.transform.rotation = Quaternion.LookRotation(xzMove);
            }
        }

        // We are in jump mode but just became grounded
        if (IsGrounded())
        {
            lastGroundedTime = Time.time;
            inAirVelocity = Vector3.zero;
            if (jumping)
            {
                jumping = false;
                //SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
            }


        }

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        m_anim.SetFloat("Walk", Mathf.Abs(v + h) * 10);
        //m_anim.SetBool("Jump", jumping);
    }
}
