using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("当目标在视野范围内返回true，否则返回false")]
    //[HelpURL("https://gitee.com/dingxiaowei/AIDemo")]
    //[TaskIcon("{SkinColor}InverterIcon.png")]
    /// <summary>
    /// 判断目标是否在视野内
    /// </summary>
    public class MyCanSeeObject : Conditional
    {
        /// <summary>
        /// 判断是否在视野内的目标
        /// </summary>
        public Transform[] Targets;
        /// <summary>
        /// 视野的角度范围
        /// </summary>
        public float FieldOfViewAngle = 90f;
        /// <summary>
        /// 视野距离
        /// </summary>
        public float ViewDistance = 7f;
        /// <summary>
        /// 不需要赋值，这个是共享找到的目标
        /// </summary>
        public SharedTransform Target;

        public override TaskStatus OnUpdate()
        {
            if (Targets == null)
                return TaskStatus.Failure;
            foreach (var target in Targets)
            {
                float distance = (target.position - transform.position).magnitude;
                float angle = Vector3.Angle(transform.forward, target.position - transform.position);
                if (distance < ViewDistance && angle < FieldOfViewAngle * 0.5f)
                {
                    this.Target.Value = target;
                    return TaskStatus.Success;
                }
            }
            return TaskStatus.Failure;
        }
    }
}
