using UnityEngine;
using System.Collections;
using Cinemachine;

public class PlayerController : MonoBehaviour {
    [Range(1,10)]
    public float AnimMoveEX;
    public Animator m_anim;
    protected CharacterController m_CharCtrl;
    public float Input_X;
    public float Input_Y;
    private Vector3 moveDirection;
    public GameObject gun;
    private FixedUpdateFollow updateFollow;
    protected bool m_IsGrounded = true;
    public float gravity = 20;
    public float maxForwardSpeed = 8f;

    const float k_GroundedRayDistance = 1f;
    const float k_GroundAcceleration = 20f;
    const float k_GroundDeceleration = 25f;
    const float k_StickingGravityProportion = 0.3f;
    const float k_JumpAbortSpeed = 10f;

    protected float m_VerticalSpeed;
    protected float m_ForwardSpeed;
    protected float m_DesiredForwardSpeed;
    protected Material m_CurrentWalkingSurface;

    [SerializeField]
    private float timesc;
    private float currentWalkTime = 0;

    protected bool IsOnTurn;

    public CinemachineFreeLook freeLook;
    protected float m_AngleDiff;
    protected Quaternion m_TargetRotation;

    protected AnimatorStateInfo m_CurrentStateInfo;
    protected AnimatorStateInfo m_NextStateInfo;
    protected bool m_IsAnimatorTransitioning;

    public float minTurnSpeed = 400f;
    public float maxTurnSpeed = 1200f;

    //readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
    //readonly int m_HashAngleDeltaRad = Animator.StringToHash("AngleDeltaRad");

    const float k_InverseOneEighty = 1f / 180f;
    
    private float turnspeed = 10f;

    // Use this for initialization
    void Start()
    {
        m_CharCtrl = GetComponent<CharacterController>();
        m_anim = GetComponent<Animator>();
        updateFollow = gun.GetComponent<FixedUpdateFollow>();
    }


    void Update()
    {
        if (AxixInput() != Vector3.zero)
        {
            RotatePlayerFaceWithCamera();
            if (m_anim.GetNextAnimatorStateInfo(0).IsName("Idle") || m_anim.GetNextAnimatorStateInfo(0).IsName("Walk"))
            {
                if (updateFollow.enabled)
                {
                    updateFollow.enabled = false;
                    updateFollow.ReturnPos();
                }
            }

            m_anim.SetBool("Walk", true);
            currentWalkTime += Time.deltaTime;

            if (currentWalkTime > 1.1f || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                m_anim.SetBool("Run", true);
            }
            else
            {
                m_anim.SetBool("Run", false);
            }

            //if (InputRotate() != 0)
            //{
            //    RotatePlayerFaceWithCamera();

            //}
        }
        else
        {
            currentWalkTime = 0;
            m_anim.SetBool("Walk", false);
            m_anim.SetBool("Run", false);
            //IsOnTurn = false;
        }
        DoAnim();

        //TimeSec(ref timesc);

    }

    private void FixedUpdate()
    {
        CalculateForwardMovement();
        CalculateVerticalMovement();
    }

    //---------
    // Called each physics step to set the rotation Ellen is aiming to have.
    void SetTargetRotation()
    {
        Vector2 p = new Vector2(AxixInput().x, AxixInput().z);
        Vector2 moveInput = AxixInput();
        Vector3 localMovementDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        Vector3 forward = Quaternion.Euler(0f, moveInput.x, 0f) * Vector3.forward;
        forward.y = 0f;
        forward.Normalize();

        Quaternion targetRotation;

        if (Mathf.Approximately(Vector3.Dot(localMovementDirection, Vector3.forward), -1.0f))
        {
            targetRotation = Quaternion.LookRotation(-forward);
        }
        else
        {
            Quaternion cameraToInputOffset = Quaternion.FromToRotation(Vector3.forward, localMovementDirection);
            targetRotation = Quaternion.LookRotation(cameraToInputOffset * forward);
        }

        Vector3 resultingForward = targetRotation * Vector3.forward;

        float angleCurrent = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
        float targetAngle = Mathf.Atan2(resultingForward.x, resultingForward.z) * Mathf.Rad2Deg;

        m_AngleDiff = Mathf.DeltaAngle(angleCurrent, targetAngle);
        m_TargetRotation = targetRotation;
    }

    bool IsOrientationUpdated()
    {
        bool updateOrientationForLocomotion = !m_IsAnimatorTransitioning;

        return updateOrientationForLocomotion;
    }

    void UpdateOrientation()
    {
        Vector3 localInput = new Vector3(AxixInput().x, 0f, AxixInput().y);
        float groundedTurnSpeed = Mathf.Lerp(maxTurnSpeed, minTurnSpeed, m_ForwardSpeed / m_DesiredForwardSpeed);
        float actualTurnSpeed = m_IsGrounded ? groundedTurnSpeed : Vector3.Angle(transform.forward, localInput) * k_InverseOneEighty * groundedTurnSpeed;
        m_TargetRotation = Quaternion.RotateTowards(transform.rotation, m_TargetRotation, actualTurnSpeed * Time.deltaTime);

        transform.rotation = m_TargetRotation;
    }

    //-------------------------

    public Vector3 AxixInput()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        return moveDirection;

    }

    public bool CheckGround()
    {
        Ray ray = new Ray(this.transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, .3f))
        {
            return true;
        }
        return false;
    }

    private void Applygravity()
    {
        float verticalSpeed;
        if (CheckGround())
        {
            verticalSpeed = 0;
        }
        else
        {
            verticalSpeed = gravity * Time.deltaTime;
            transform.position -= new Vector3(0, verticalSpeed, 0);
        }
    }

    public float InputRotate()
    {
        //return Input.GetAxis("Horizontal") != 0 ? true : false;
        return Input.GetAxis("Horizontal");
    }

    public void DoAnim()
    {
        m_anim.SetFloat("v", Input.GetAxis("Vertical"));
        m_anim.SetFloat("h", Input.GetAxis("Horizontal"));
    }

    private void RotatePlayerFaceWithCamera()
    {
        float H = Input.GetAxis("Horizontal");
        float V = Input.GetAxis("Vertical");
        Vector3 inputVec = new Vector3(H, 0, V).normalized;
        Vector3 playerFace = Camera.main.transform.TransformDirection(inputVec);
        Quaternion _rotation = Quaternion.LookRotation(playerFace);
        _rotation.x = 0;
        _rotation.z = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation,_rotation,Time.deltaTime*turnspeed);
    }

    private IEnumerator PlayShoot()
    {
        if (m_anim.GetLayerWeight(1) == 1)
        {
            yield return null;
        }
        else
        {
            m_anim.SetLayerWeight(1, 1);
            m_anim.Play("Shoot", 1, 0);
            updateFollow.enabled = true;
            updateFollow.StartCoroutine(updateFollow.Shoot());
            gun.GetComponent<Animator>().Play("Anim", 0, -.5f);
            yield return new WaitForSeconds(1f);
            m_anim.SetLayerWeight(1, 0);
        }

    }

    void CalculateForwardMovement()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (moveInput.sqrMagnitude > 1f)
            moveInput.Normalize();

        m_DesiredForwardSpeed = moveInput.magnitude * maxForwardSpeed;

        float acceleration = AxixInput().sqrMagnitude > 0 ? k_GroundAcceleration : k_GroundDeceleration;

        m_ForwardSpeed = Mathf.MoveTowards(m_ForwardSpeed, m_DesiredForwardSpeed, acceleration * Time.deltaTime);

        m_anim.SetBool("Walk", m_ForwardSpeed > 0 ? true : false);
    }

    // Called each physics step.
    void CalculateVerticalMovement()
    {
        if (m_IsGrounded)
        {
            m_VerticalSpeed = -gravity * k_StickingGravityProportion;
        }
        else
        {
            if (m_VerticalSpeed > 0.0f)
            {
                m_VerticalSpeed -= k_JumpAbortSpeed * Time.deltaTime;
            }

            if (Mathf.Approximately(m_VerticalSpeed, 0f))
            {
                m_VerticalSpeed = 0f;
            }

            m_VerticalSpeed -= gravity * Time.deltaTime;
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 movement;

        if (m_IsGrounded)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position + Vector3.up * k_GroundedRayDistance * 0.5f, -Vector3.up);
            if (Physics.Raycast(ray, out hit, k_GroundedRayDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                movement = Vector3.ProjectOnPlane(m_anim.deltaPosition* AnimMoveEX, hit.normal);

                Renderer groundRenderer = hit.collider.GetComponentInChildren<Renderer>();
                m_CurrentWalkingSurface = groundRenderer ? groundRenderer.sharedMaterial : null;
            }
            else
            {
                movement = m_anim.deltaPosition* AnimMoveEX;
                m_CurrentWalkingSurface = null;
            }
        }
        else
        {
            movement = m_ForwardSpeed * transform.forward * Time.deltaTime;
        }

        m_CharCtrl.transform.rotation *= m_anim.deltaRotation;

        movement += m_VerticalSpeed * Vector3.up * Time.deltaTime;

        m_CharCtrl.Move(movement);

        m_IsGrounded = m_CharCtrl.isGrounded;

        if (!m_IsGrounded)
            m_anim.SetBool("Walk", m_IsGrounded);

    }

    private void TimeSec(ref float timescnum)
    {
        timescnum = Time.timeScale;
    }
}
