using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskDescription("转向目标点")]
public class MyRotate : Action
{
    public EnemyController Enemy;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("目标朝向")]
    public Vector3 TargetForwardPosisition;
    public float TurnSpeed = 3f;

    public override void OnAwake()
    {
        if (Enemy == null)
            Enemy = transform.GetComponent<EnemyController>();
    }
    public override TaskStatus OnUpdate()
    {
        Vector3 v1 = TargetForwardPosisition - transform.position;
        v1.y = 0;
        float angle = Vector3.Angle(transform.forward, v1);
        if (angle < 0.1)
        {
            return TaskStatus.Success;
        }
        Vector3 cross = Vector3.Cross(transform.forward, v1);
        transform.Rotate(cross, Mathf.Min(TurnSpeed, Mathf.Abs(angle)));
        return TaskStatus.Running;
    }
}
