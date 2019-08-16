using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 03 控制ai角色移動，
// 他能真正控制AI角色移動，包含計算每次移動距離，撥放動畫等
public class AILocomotion : Vehicle
{
    //AI角色的控制器
    private CharacterController controller;
    //AI角色的Rigidbody
    private Rigidbody therigidbody;
    //AI角色每次的移動距離
    private Vector3 moveDistance;

    void Start()
    {
        //獲得角色控制器(如果有的畫)
        controller = GetComponent<CharacterController>();
        //獲得AI角色的Rigidbody(如果有的話)
        therigidbody = GetComponent<Rigidbody>();
        moveDistance = new Vector3(0, 0, 0);

        //呼叫基礎類別的start()函數，進行所需的初始化
        base.Start();
        RoleCheck();
    }

    //實體相關操作在FixUpdate()中更新
    private void FixedUpdate()
    {
        if(!IsPlayer)
        {
            //計算速度
            velocity += acceleration * Time.fixedDeltaTime;
            //限制速度，需低於最大速度
            if (velocity.sqrMagnitude > sqrMaxSpeed)
                velocity = velocity.normalized * maxSpeed;
            //計算AI角色的移動距離
            moveDistance = velocity * Time.fixedDeltaTime;
            //如果要求AI角色在平面上移動，將y置為0
            if (isPlanar)
            {
                velocity.y = 0;
                moveDistance.y = 0;
            }

            //如果已經為AI角色增加了控制器，那麼利用角色控制器使其移動
            if (controller != null)
                controller.SimpleMove(velocity);
            //如果AI角色沒有Rigidbody，也沒有控制器
            //或AI角色有Rigidbody，但是要由動力學的方式控制它移動
            else if (therigidbody == null || therigidbody.isKinematic)
                transform.position += moveDistance;
            //用Rigidbody控制AI角色的運動
            else
                therigidbody.MovePosition(therigidbody.position + moveDistance);
            //更新朝向，如果速度大於一個設定值(防止抖動)
            if (velocity.sqrMagnitude > 0.00001)
            {
                //透過目前朝向與速度方向內插，計算新地朝向
                Vector3 newFoeward = Vector3.Slerp(transform.forward, velocity, damping * Time.deltaTime);
                //將y設定為0
                if (isPlanar)
                    newFoeward.y = 0;
                //將向前的方向設定為新的朝向
                transform.forward = newFoeward;
            }
            //撥放行走動畫
            //Animation anim;
            //anim = gameObject.GetComponent<Animation>();
            //anim.Play("walk");
        }



    }

    private bool IsPlayer;
    private bool IsAI;
    //check if ai or player
    void RoleCheck()
    {
        if(this.gameObject.name == "Player")
        {
            IsPlayer = true;
        }
    }

}
