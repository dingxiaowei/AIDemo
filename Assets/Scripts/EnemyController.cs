using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float Speed = 3f;
    public float TurnSpeed = 3f;
    public Vector3 BasePosition;
    public Vector3 BaseForward;

    public GameObject bullet;
    public float BulletSpeed = 30f;
    public float FireInterval = 0.2f;
    public float FireCd = 0f;
    //public GameObject BeShotShooter = null;

    public int hp = 10;
    public int MaxHp = 10;
    public const float MinHpPersent = 0.2f;

    public UnityEngine.AI.NavMeshAgent navMeshAgent;

    /// <summary>
    /// 防卫点
    /// </summary>
    public GameObject[] fortress;

    public void Awake()
    {
        BasePosition = transform.position;
        BaseForward = transform.forward;
        bullet = Resources.Load("Prefabs/EnemyBullet") as GameObject;
        fortress = GameObject.FindGameObjectsWithTag("Fortress");
        navMeshAgent = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void Fire()
    {
        if (FireCd > Time.time)
        {
            return;
        }
        var b = Instantiate(bullet, gameObject.transform.position, Quaternion.identity, transform);
        var rigid = b.GetComponent<Rigidbody>();
        rigid.velocity = transform.forward * BulletSpeed;
        FireCd = Time.time + FireInterval;
        var bulletScript = b.GetComponent<Bullet>();
        //bulletScript.Shooter = gameObject;
    }

    public void RotateTo(Vector3 pos)
    {
        Vector3 v1 = pos - transform.position;
        v1.y = 0;
        float angle = Vector3.Angle(transform.forward, v1);
        if (angle < 0.1)
        {
            return;
        }
        Vector3 cross = Vector3.Cross(transform.forward, v1);
        transform.Rotate(cross, Mathf.Min(TurnSpeed, Mathf.Abs(angle)));
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject bullet = other.transform.gameObject;
        if (bullet.tag != "PlayerBullet")
        {
            return;
        }
        //BeShotShooter = bullet.GetComponent<Bullet>().Shooter;
        Destroy(bullet);
        if (hp > 0)
        {
            hp -= 1;
            Debug.LogError("Left Hp:"+hp.ToString());
            if (hp == 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        var com = gameObject.GetComponent("BehaviorTree");
        Destroy(com);
        // 如果不删除寻路组件，就不能旋转
        var com2 = gameObject.GetComponent("NavMeshAgent");
        Destroy(com2);
        transform.Rotate(transform.right, 90);
    }

    public bool EnemyHpIsLower()
    {
        if (hp <= MaxHp * MinHpPersent)
        {
            return true;
        }
        return false;
    }
}
