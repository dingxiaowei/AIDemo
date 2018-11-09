using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[TaskDescription("逃跑")]
public class MyFlee : Action
{
  
    EnemyController enemyController;

    public override void OnAwake()
    {
        enemyController = this.gameObject.GetComponent<EnemyController>();
    }

    public override void OnStart()
    {
        float speed = Random.Range(5f, 6f);
        enemyController.navMeshAgent.speed = speed;
        enemyController.navMeshAgent.angularSpeed = 0;
       
    }

    public override TaskStatus OnUpdate()
    {
        if (enemyController == null || enemyController.fortress.Length == 0 || PlayerController1.IsDie)
        {
            return TaskStatus.Failure;
        }
        float dis_for2 = Vector3.Distance(transform.localPosition, enemyController.fortress[0].transform.localPosition);
        if (dis_for2 < 1.2f)
        {
            return TaskStatus.Success;
        }
        else
        {
            if (enemyController.EnemyHpIsLower())
            {
                if (enemyController.navMeshAgent.enabled && !enemyController.navMeshAgent.pathPending && enemyController.navMeshAgent.remainingDistance < 1f)
                {
                    return TaskStatus.Success;
                }

                if (Vector3.Distance(this.transform.localPosition, enemyController.fortress[0].transform.localPosition) < 1f)
                    return TaskStatus.Success;
                else
                {
                    enemyController.navMeshAgent.enabled = true;
                    enemyController.navMeshAgent.SetDestination(enemyController.fortress[0].transform.position);
                    this.transform.LookAt(enemyController.fortress[0].transform);
                    return TaskStatus.Running;
                }
            }

        }
        return TaskStatus.Success;
    }

}
