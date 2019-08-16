using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class FlyAvatarControl : MonoBehaviour {
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    public float speed = 6.0f;

    private bool isControllable = true;

    private float verticalSpeed = 0.0f;
    // The current x-z move speed
    private float moveSpeed = 0.0f;

    private bool isMoving = false;

    private bool movingBack = false;

    float inAirControlAcceleration = 3.0f;
    private float lockCameraTimer = 0.0f;

    float walkSpeed = 2.0f;
    float trotSpeed = 4.0f;
    float runSpeed = 12.0f;//6.0
    float speedSmoothing = 10.0f;
    float rotateSpeed = 500.0f;
    float trotAfterSeconds = 3.0f;
    private float walkTimeStart = 0.0f;

    public bool UseObs;

    void Start()
    {
        controller = GetComponent<CharacterController>();

    }

    private void Update()
    {
        //if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        //{
        //    moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        //    moveDirection = transform.TransformDirection(moveDirection);
        //    moveDirection = moveDirection * speed;

        //    //Vector3 relativePos = moveDirection - transform.position;
        //    transform.rotation = new Quaternion(0, Input.GetAxis("Horizontal"), 0 , 1);

        //}
        //controller.Move(moveDirection * Time.deltaTime);
        FlyMotion();
    }

    float gravity = 20;
    void ApplyObstruction()
    {
        float DistanceBuff = Vector3.Distance(new Vector3(0, this.transform.position.y, 0), new Vector3(0, 3, 0));
        if(this.transform.position.y>3)
        {
            //verticalSpeed -= gravity*Time.deltaTime/2;
            verticalSpeed -= (gravity * Time.deltaTime*DistanceBuff);
        }
        else
        {
            //verticalSpeed = 0;
            //verticalSpeed = (Time.time % 5) / 20;
            verticalSpeed += (gravity * Time.deltaTime * DistanceBuff);
        }
    }

    void UpdateSmoothFlyMovement()
    {
        Transform cameraTransform = Camera.main.transform;

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
        Vector3 targetDirection = h * right + v * forward;

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
            if (moveSpeed < walkSpeed * 0.9f)
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


        // Pick speed modifier
        if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift) | isMoving)
        {
            targetSpeed *= runSpeed;
        }
        else if (Time.time - trotAfterSeconds > walkTimeStart)
        {
            targetSpeed *= trotSpeed;
        }
        else
        {
            targetSpeed *= walkSpeed;

        }

        moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);

        // Reset walk time start when we slow down
        if (moveSpeed < walkSpeed * 0.3f)
            walkTimeStart = Time.time;

        //// In air controls
        //else
        //{
        //    // Lock camera while in air
        //    if (jumping)
        //        lockCameraTimer = 0.0f;

        //    if (isMoving)
        //        inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
        //}

    }

    bool IsMoving()
    {
        return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
    }

    void FlyMotion()
    {

        if (!isControllable)
        {
            // kill all inputs if not controllable.
            Input.ResetInputAxes();
        }


        UpdateSmoothFlyMovement();
        if(UseObs)
        {
            ApplyObstruction();
        }
        
        if (IsMoving())
        {
            var newPos = transform.position + (transform.rotation * Vector3.forward * moveSpeed);
            //newPos.y = Terrain.activeTerrain.SampleHeight(newPos);

            var heropos = transform.position;
            //heropos.y = Terrain.activeTerrain.SampleHeight(transform.position);
            Debug.DrawLine(heropos, newPos, Color.red);

            var c = moveSpeed;
            var b = newPos.y - heropos.y;
            if (b > 0)
            {
                var a = Mathf.Sqrt(Mathf.Pow(c, 2) - Mathf.Pow(b, 2));
                moveSpeed = a;
            }
        }


        // Calculate actual motion
        Vector3 movement = moveDirection * moveSpeed + new Vector3(0, verticalSpeed, 0);
        movement *= Time.deltaTime;

        controller.Move(movement);
        transform.rotation = Quaternion.LookRotation(moveDirection);
        // Set rotation to the move direction
        //if ()
        //{

        //    transform.rotation = Quaternion.LookRotation(moveDirection);

        //}
        //else
        //{
        //    Vector3 xzMove = movement;
        //    xzMove.y = 0;
        //    if (xzMove.sqrMagnitude > 0.001f)
        //    {
        //        transform.rotation = Quaternion.LookRotation(xzMove);
        //    }
        //}


        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        //m_anim.SetFloat("Walk", Mathf.Abs(v + h) * 10);
        //m_anim.SetBool("Jump", jumping);
    }

}
