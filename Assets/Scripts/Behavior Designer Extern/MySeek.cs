using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using BehaviorDesigner.Runtime;
namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("跟随目标,当达到目标一定距离范围则返回true，否则返回正在running")]
    [HelpURL("https://gitee.com/dingxiaowei/AIDemo")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
    public class MySeek : Action //任务的调用由Behavior Designer行为树控制的
    {
        [Tooltip("Angular speed of the agent")]
        public SharedFloat AngularSpeed;
        /// <summary>
        /// 要达到的目标位置
        /// </summary>
        public SharedTransform TargetTransform;  //设置Shared类型可以通过行为树里面变量来赋值
        public float Speed;
        
        /// <summary>
        /// 到达目标位置的距离
        /// </summary>
        [Tooltip("The agent has arrived when the magnitude is less than this value")]
        public float ArriveDistance = 0.1f;
        [Tooltip("If target is null then use the target position")]
        public SharedVector3 TargetPosition;
        private float SeekMaxDis = 5f;
        public SharedGameObject homePos;
        public float speed = 3.5f;
        public float offset = 0.8f;
        EnemyController enemyController;


        // True if the target is a transform
        private bool dynamicTarget;

        private UnityEngine.AI.NavMeshAgent navMeshAgent;

        public bool DynamicTarget
        { 
            get
            {
                return (TargetTransform != null && TargetTransform.Value != null);
            }
        }

        private Vector3 Target()
        {
            Vector3 offVect = new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset));//Home位置随机离散;
            if (dynamicTarget)
            {
                Debug.LogError("goto target");
                return TargetTransform.Value.localPosition + offVect;//Target位置随机离散
            }
            else
            {
                Debug.LogError("goto home");
                return TargetPosition.Value + offVect;//Home位置随机离散;
            }
        }
        public override void OnAwake()
        {
            // cache for quick lookup
            navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            navMeshAgent.speed = speed;
            navMeshAgent.angularSpeed = AngularSpeed.Value;
            navMeshAgent.enabled = true;
            //navMeshAgent.destination = Target();
            enemyController = this.gameObject.GetComponent<EnemyController>();
        }


        /// <summary>
        /// 每次重新进入的时候都会重新计算
        /// </summary>
        public override void OnStart()
        {
            homePos = GameObject.FindGameObjectWithTag("Home");
            speed = Random.Range(2f, 6f);
            dynamicTarget = (TargetTransform != null && TargetTransform.Value != null);
        }


        // Seek the destination. Return success once the agent has reached the destination.
        // Return running if the agent hasn't reached the destination yet
        public override TaskStatus OnUpdate()
        {
            if (PlayerController1.IsDie)
                return TaskStatus.Failure;
            if (navMeshAgent.enabled && navMeshAgent.destination!=null && !navMeshAgent.pathPending && navMeshAgent.remainingDistance < ArriveDistance)
            {
                //Debug.LogError("寻路结束");
                navMeshAgent.isStopped = true;
                return TaskStatus.Success;
            }

            // Update the destination if the target is a transform because that agent could move
            if (TargetTransform.Value != null)
            {
                //navMeshAgent.destination = Target();

                float dis = Vector3.Distance(TargetTransform.Value.localPosition, this.transform.localPosition);
                if (dis > SeekMaxDis)
                {
                    return TaskStatus.Failure;
                }
                else
                {
                    if (TargetTransform.Value != null)
                    {
                        if (dis > 0.5f)
                        {
                            transform.LookAt(TargetTransform.Value.transform);
                            navMeshAgent.enabled = true;
                            navMeshAgent.destination = Target();
                            //Debug.LogError("跟随player");
                            return TaskStatus.Running;
                        }
                        else
                        {
                            return TaskStatus.Success;
                        }
                    }
                }
            }
            else
            {
                if (homePos.Value != null)
                {
                    float dis = Vector3.Distance(homePos.Value.transform.localPosition, this.transform.localPosition);
                    if (dis < 0.3f)
                    {
                        if (this.transform.localRotation.eulerAngles != new Vector3(0, 180, 0))
                        {
                            this.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
                            Debug.LogError("到老家了，换个姿势");
                            return TaskStatus.Success;
                        }
                        return TaskStatus.Success;

                    }
                    else
                    {
                        float dis_for2 = Vector3.Distance(transform.localPosition, enemyController.fortress[0].transform.localPosition);
                        if (dis_for2 < 1.2f)
                        {
                            if (this.transform.localEulerAngles != new Vector3(0, 210, 0))
                            {
                                this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, Quaternion.Euler(new Vector3(0, 210, 0)), 1f);
                                return TaskStatus.Success;
                            }
                            return TaskStatus.Failure;
                        }

                        transform.LookAt(homePos.Value.transform);
                        navMeshAgent.enabled = true;
                        navMeshAgent.destination = homePos.Value.transform.localPosition;
                        return TaskStatus.Running;
                    }
                }
                else
                {
                    Debug.LogError("homePos is null");
                }
            }
            return TaskStatus.Failure;
        }

        public override void OnEnd()
        {
            // Disable the nav mesh
            navMeshAgent.enabled = false;
        }
    }
}
