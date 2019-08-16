using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlMove {
    private Animator m_anim;
    public GameObject Player;
    private CharacterController col = null;
    private float moveSpeed = 1f;
    private Vector3 moveDirection;
    private AudioController audioController;
    private CollisionFlags collisionFlags;
    private float verticalSpeed;
    private float gravity = 20;

    public void Initialized()
    {
        col = Player.GetComponent<CharacterController>();
        m_anim = Player.transform.GetChild(0).GetComponent<Animator>();
        //moveDirection = Player.transform.TransformDirection(Vector3.forward);
        m_anim = Player.transform.GetChild(0).GetComponent<Animator>();
        //m_anim = Player.GetComponent<Animator>();
        audioController = Player.GetComponent<AudioController>();
    }

    // Update is called once per frame
    public void Update()
    {
        ApplyGravity();
        if (AxixInput() != Vector3.zero)
        {
            //m_anim.SetBool("Walk", true);
            Vector3 walk = new Vector3();
            Transform cameraTransform = Camera.main.transform;
            //Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
            if (AxixInput().z > 0)
            {
                //walk = Player.transform.TransformDirection(Vector3.forward);
                walk = cameraTransform.TransformDirection(Vector3.forward);
            }
            else if (AxixInput().z < 0)
            {
                //walk = Player.transform.TransformDirection(Vector3.back);
                walk = cameraTransform.TransformDirection(Vector3.back);
            }
            walk.y = 0;
            Vector3 lookForwardP =  2 * walk;
            Player.transform.rotation = Quaternion.LookRotation(lookForwardP);
            Vector3 movement = walk + new Vector3(0, verticalSpeed, 0);
            collisionFlags = col.Move(walk * moveSpeed);
           
            if (InputRotate())
            {
                Player.transform.rotation *= new Quaternion(0, Input.GetAxis("Horizontal") / 10, 0, 1);
            }
        }
        else
        {
            //m_anim.SetBool("Walk", false);
        }

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        m_anim.SetFloat("Walk", Mathf.Abs(v + h) * 10);
        
    }

    public Vector3 AxixInput()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        return moveDirection;

    }
    public bool InputRotate()
    {
        return Input.GetAxis("Horizontal") != 0 ? true : false;
    }

    public void ApplyGravity()
    {
        if (IsGrounded())
            verticalSpeed = 0.0f;
        else
            verticalSpeed -= gravity * Time.deltaTime;
    }

    bool IsGrounded()
    {
        return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }
}
