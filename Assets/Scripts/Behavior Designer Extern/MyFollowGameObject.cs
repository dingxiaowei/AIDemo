using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskDescription("跟随目标物体")]

public class MyFollowGameObject : Conditional
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("追击的最小距离")]
    public float minReachDis = 2f;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("追击速度")]
    public float speed = 3f;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("追击的最大距离")]
    public float SeekDistance = 5f;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("共享主角")]
    public SharedGameObject Player;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("共享Home")]
    public SharedGameObject Home;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private EnemyController enemyController;


    public override void OnAwake()
    {
        // cache for quick lookup
        navMeshAgent = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        enemyController = this.GetComponent<EnemyController>();
    }

    /// <summary>
    /// 每次重新进入的时候都会重新计算
    /// </summary>
    public override void OnStart()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Home = GameObject.FindGameObjectWithTag("Home");

        navMeshAgent.speed = 3;
        navMeshAgent.angularSpeed = 0;
    }


    public override TaskStatus OnUpdate()
    {
        if (null == Player)
            return TaskStatus.Failure;

        float dis = Vector3.Distance(Player.Value.transform.localPosition, this.transform.localPosition);

        if (dis <= minReachDis)
        {
            //Debug.Log("老子追上了，干");
            return TaskStatus.Success;
        }
        else if (dis > minReachDis && dis < SeekDistance)
        {
            //Debug.LogError("跟随player");    
            transform.LookAt(Player.Value.transform);
            if (!enemyController.EnemyHpIsLower())
            {
                navMeshAgent.enabled = true;
                navMeshAgent.destination = Player.Value.transform.localPosition;
                return TaskStatus.Running;
            }
            else
                return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Success;
        }
    }
}
