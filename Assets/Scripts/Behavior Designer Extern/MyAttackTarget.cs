using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[TaskDescription("Attack Target")]
public class MyAttackTarget : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("共享的主角")]
    public SharedGameObject Player;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("共享的Home")]
    public SharedGameObject Home;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("子弹发射点")]
    private Transform bullet_startPos;
    /// <summary>
    /// 子弹预制
    /// </summary>
    public GameObject prb_bullet;
    /// <summary>
    /// 攻击距离
    /// </summary>
    public float AttackDis = 5f;


    public override void OnStart()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Home = GameObject.FindGameObjectWithTag("Home");
    }

    public override TaskStatus OnUpdate()
    {
        float dis = Vector3.Distance(Player.Value.transform.localPosition, this.transform.localPosition);
        if (dis > AttackDis)
        {
            //Debug.Log("超过攻击范围");
            return TaskStatus.Failure;
        }
        else
        {
            if (Time.frameCount % 10 == 0)
            {
                this.gameObject.transform.LookAt(Player.Value.transform);
                GameObject go = GameObject.Instantiate(prb_bullet);
                go.transform.localPosition = this.transform.Find("Face").position;
                go.GetComponent<BulletMgr>().Target = Player.Value;

                GameObject.Destroy(go, 3f);
                return TaskStatus.Running;
            }
        }

        return TaskStatus.Success;
    }

}
