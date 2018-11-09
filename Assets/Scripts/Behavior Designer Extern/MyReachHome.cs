using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 是否到达指定位置
/// TODO:位置可动态指定
/// </summary>
[TaskDescription("是否到家(一级堡垒点)")]
public class MyReachHome : Action
{
    public SharedGameObject Home;
    public float ReachDis = 0.5f;

    public override void OnStart()
    {
        Home = GameObject.FindGameObjectWithTag("Home");
    }


    public override TaskStatus OnUpdate()
    {
        if (Home.Value != null)
        {
            float dis = Vector3.Distance(transform.localPosition, Home.Value.transform.localPosition);
            if (dis < ReachDis)
            {
                if (this.transform.localEulerAngles != new Vector3(0, 180, 0))
                {
                    this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, Quaternion.Euler(new Vector3(0, 180, 0)), 1f);
                    return TaskStatus.Success;
                }
            }
        }
        return TaskStatus.Failure;
    }

}
