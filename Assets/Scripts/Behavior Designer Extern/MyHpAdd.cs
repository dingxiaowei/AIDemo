using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[TaskDescription("Auto Add HP")]
public class MyHpAdd : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("EnemyControler")]
    private EnemyController enemyController;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("Last Add Hp TimeStamp")]
    private float timeStamp;


    public override void OnAwake()
    {
        enemyController = this.gameObject.GetComponent<EnemyController>();
        timeStamp = Time.realtimeSinceStartup;
    }

    public override TaskStatus OnUpdate()
    {
        if (enemyController.hp < enemyController.MaxHp)
        {
            if (Time.realtimeSinceStartup - timeStamp > 2f) //2秒回血+1
            {
                enemyController.hp++;
                timeStamp = Time.realtimeSinceStartup;
                Debug.LogError("Enenmy Hp:" + enemyController.hp.ToString());
                return TaskStatus.Running;
            }
        }
        else
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
