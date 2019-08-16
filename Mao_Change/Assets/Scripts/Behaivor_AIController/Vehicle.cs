using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 02 將AI角色抽象成一個質點，
// 它包含位置，品質，速度等資訊，是實現操控行為的基礎
public class Vehicle :MonoBehaviour
{
    //AI角色包含的操控行為列表
    public Steering[] steerings;
    //AI角色能到達的最大速率
    public float maxSpeed = 10;
    //設定能施加捯這個AI角色的力的最大值
    public float maxForce = 100;
    //最大速度的平方，透過預先算出並儲存，節省資源
    protected float sqrMaxSpeed;
    //AI角色的品質
    public float mass = 1;
    //AI角色的速度
    public Vector3 velocity;
    //控制轉向的速度
    public float damping = 0.9f;
    //操控力的計算間隔時間，為了達到更高的每秒顯示畫面，操控力不需要每個畫面更新
    public float computeInterval = 0.2f;
    //是某在二維平面上，如果是，計算兩個GAMEObject的距離時，忽略Y值的不同
    public bool isPlanar = true;
    //計算獲得的操縱力
    private Vector3 steeringForce;
    //AI角色的加速度
    protected Vector3 acceleration;
    //計時器
    private float timer;

	// Use this for initialization
	protected void Start ()
    {
        steeringForce = new Vector3(0, 0, 0);
        sqrMaxSpeed = maxSpeed * maxSpeed;
        timer = 0;

        //獲得這個AI角色所包含的操控行為列表
        steerings = gameObject.GetComponents<Steering>();
	}

    //public void Initialized()
    //{
    //    steeringForce = new Vector3(0, 0, 0);
    //    sqrMaxSpeed = maxSpeed * maxSpeed;
    //    timer = 0;

    //    //獲得這個AI角色所包含的操控行為列表
    //    steerings[0] = new SteeringForLeaderFollowing();
    //    steerings[1] = new SteeringForArrive();
    //    steerings[2] = new SteeringForCohesion();
    //    steerings[3] = new SteeringForEvade();
    //    steerings[4] = new SteeringForPursuit();
    //    steerings[5] = new SteeringForSeparation();
    //    steerings[6] = new SteeringForWander();
    //}

    // Update is called once per frame
    public void Update ()
    {
        timer += Time.deltaTime;
        steeringForce = new Vector3(0, 0, 0);

        //如果距離上次計算操控力的時間大於設定的時間間格computeInterval;
        //再次計算操控力
        if(timer > computeInterval)
        {
            //將操控行為列表中的所有操控行為對應的操控力行帶全數的求和
            foreach(Steering s in steerings)
            {
                if (s.IsEnable)
                    steeringForce += s.Force() * s.weight;
            }

            //操控力不大於maxForce;
            steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);

            //力除以品質，求出加速度;
            acceleration = steeringForce / mass;

            //重新從零開始計時;
            timer = 0;
        }
		
	}
}
