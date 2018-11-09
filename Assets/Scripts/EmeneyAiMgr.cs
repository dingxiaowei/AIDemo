using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmeneyAiMgr : MonoBehaviour
{
    public GameObject prbEnemy;
    public int EnemyNumber;
    private SharedTransform Target;
    BehaviorTree bt;


    // Use this for initialization
    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < EnemyNumber; i++)
        {
            GameObject enemy = GameObject.Instantiate(prbEnemy);
            enemy.transform.parent = this.transform;
            enemy.transform.localPosition = new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-2, 2f));

            enemy.AddComponent<EnemyController>();
            var bt = enemy.AddComponent<BehaviorTree>();
            var extBt = Resources.Load<ExternalBehaviorTree>("BehaveAI/EnemyAI_001");
            bt.StartWhenEnabled = true;
            bt.PauseWhenDisabled = true;
            bt.RestartWhenComplete = true;
            bt.ResetValuesOnRestart = true;
            bt.ExternalBehavior = extBt;
            bt.EnableBehavior();
        }
    }

}
