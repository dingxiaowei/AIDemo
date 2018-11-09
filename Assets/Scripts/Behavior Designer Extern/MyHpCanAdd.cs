using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[TaskDescription("Check Can Add HP Or Not")]
public class MyHpCanAdd : Conditional
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("EnemyControler")]
    private EnemyController enemyController;


    public override void OnAwake()
    {
        enemyController = this.gameObject.GetComponent<EnemyController>();
    }

    public override TaskStatus OnUpdate()
    {
        if (PlayerController1.IsDie)
            return TaskStatus.Failure;

        //The Distance to fortress level 2;
        float dis_for2 = Vector3.Distance(transform.localPosition, enemyController.fortress[0].transform.localPosition);
        if (dis_for2 < 1.2f)
        {
            if (enemyController.hp < enemyController.MaxHp)
                return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
