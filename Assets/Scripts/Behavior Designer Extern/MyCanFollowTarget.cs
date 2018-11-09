using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BehaviorDesigner.Runtime.Tasks.TaskDescription("是否可跟随")]
public class MyCanFollowTarget : Conditional
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("追击范围")]
    public float SeekDistance = 5f;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("共享主角")]
    public SharedGameObject Player;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("共享的Home")]
    public SharedGameObject Home;
    /// <summary>
    /// EnemyControler
    /// </summary>
    private EnemyController enemyController;


    public override void OnAwake()
    {
        enemyController = this.GetComponent<EnemyController>();
    }

    public override void OnStart()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Home = GameObject.FindGameObjectWithTag("Home");
    }


    public override TaskStatus OnUpdate()
    {
        if (Player == null || PlayerController1.IsDie)
            return TaskStatus.Failure;
        float dis2Fortress2 = Vector3.Distance(transform.localPosition, enemyController.fortress[0].transform.localPosition);
        if (dis2Fortress2 < 2f)//在二级堡垒点
        {

            float dis2Player = Vector3.Distance(Player.Value.transform.localPosition, this.transform.localPosition);
            if (dis2Player < SeekDistance)
            {
                //Debug.LogError("到达二级堡垒点,死也要干");
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
        else
        {
            float dis2Player = Vector3.Distance(Player.Value.transform.localPosition, this.transform.localPosition);
            if (dis2Player > SeekDistance)//超过追击范围
            {
                if ((Vector3.Distance(Home.Value.transform.localPosition, this.transform.localPosition) < 0.2f))
                    return TaskStatus.Failure;
                return TaskStatus.Failure;
            }
            else
            {
                if (enemyController.EnemyHpIsLower())
                    return TaskStatus.Failure;
                else
                    return TaskStatus.Success;
            }
        }
    }


}
