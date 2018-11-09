using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMgr :MonoBehaviour
{
    public GameObject Target;
    public float speed = 12f;

    // Update is called once per frame
    void Update()
    {
        if (Target)
        {
            transform.LookAt(Target.transform);
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, Target.transform.localPosition, Time.deltaTime * speed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.LogError("击中Player,子弹消失");
            Destroy(this.gameObject);
        }
    }
}
