using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public float MoveSpeed = 8f;
    public GameObject Bullet;
    public float BulletSpeed = 30f;
    public float fireinterval = 0.3f;
    private float fireCd = 0;
    int hp = 100;
    Rigidbody rigibody;
    int goundMask;
    public static bool IsDie;

    void Start()
    {
        rigibody = GetComponent<Rigidbody>();
        goundMask = LayerMask.GetMask("Ground");
        if (Bullet == null)
        {
            Bullet = Resources.Load("Prefabs/PlayerBullet") as GameObject;
        }
    }

    void Fire()
    {
        if (fireCd > Time.time)
        {
            return;
        }

        var b = Instantiate(Bullet, transform.position, Quaternion.identity, null);
        var rigid = b.GetComponent<Rigidbody>();
        rigid.velocity = transform.forward * BulletSpeed;
        fireCd = Time.time + fireinterval;
        var src = b.GetComponent<Bullet>();
        //src.Shooter = this.gameObject;
    }

    void Update()
    {
        if (hp > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            Physics.Raycast(ray, out hit, 100, goundMask);
            if (hit.transform != null)
            {
                transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
            }
            rigibody.velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * MoveSpeed;

            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
        }
    }

    void Die()
    {
        transform.Rotate(transform.right, 80);
        rigibody.velocity = Vector3.zero;
        IsDie = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject bullet = other.transform.gameObject;
        if (bullet.tag != "EnemyBullet")
        {
            return;
        }
        Destroy(bullet);
        if (hp > 0)
        {
            hp -= 1;
            if (hp == 0)
            {
                Die();
            }
        }
    }
}
