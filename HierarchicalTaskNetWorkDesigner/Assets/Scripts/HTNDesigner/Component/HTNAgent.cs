using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using HTNDesigner.BlackBoard;
using HTNDesigner.Data;
using HTNDesigner.DataStructure;
using HTNDesigner.Domain;
using HTNDesigner.Tick;
using UnityEngine;
using UnityEngine.AI;


namespace HTNDesigner.Component
{
    public class HTNAgent : MonoBehaviour, ITaskAgent
    {
        private PlanMaker _planMaker;
        private HTNRunner _planRunner;
        [SerializeField]private WorldStateBlackBoard _worldState;
        private WorldStateBlackBoard _copy;
        private TaskTree _taskTree;
        [SerializeField] private TaskTreeBuilder _treeData;
        private TaskStatus _curStatus;



        #region 测试

        
        private AsyncReactiveProperty<bool> test = new AsyncReactiveProperty<bool>(false);
        [SerializeField] private bool testVal;
        private Vector3 oriPos;

        private void OnValidate()
        {
            try
            {
                test.Value = testVal;
            }
            catch           
            {
            }
        }

        private void Awake()
        {
            oriPos = transform.position;
            test.Subscribe(Test);
        }

        public void Test(bool val)
        {
            if (val)
            {
                
                GetComponent<NavMeshAgent>().speed = 2;
                RePlaning();
            }
            else
            {
                Debug.LogError("重置");
                GetComponent<NavMeshAgent>().isStopped = true;
                GetComponent<NavMeshAgent>().ResetPath();
                GetComponent<NavMeshAgent>().speed = 0;
                transform.position = oriPos;
                _planMaker = new PlanMaker(this);
                _planRunner = new HTNRunner(this);
                _worldState.ResetWorld();
            }
        }

        public void ResetState()
        {
            
        }
        #endregion
        private void Start()
        {
            _worldState.WorldStateChageEvent += () =>
            {
                _curStatus = TaskStatus.TASK_FALIURE;
            };
        }

        private void Update()
        {
            if (test.Value)
            {
                if (_curStatus != TaskStatus.TASK_FALIURE)
                    _curStatus = _planRunner.Update();
                else
                    RePlaning();
            }
        }

        private void RePlaning()
        {
            var nPlan = _planMaker.MakePlan();
            _planRunner.StartRun(nPlan);
            _curStatus = TaskStatus.TASK_RUNNIGN;
        }

        public GameObject TaskInstance { get=>this.gameObject; }
        public TaskNode m_Root { get => _treeData.m_Tree.m_RootNode; }

        public WorldStateBlackBoard m_worldState
        {
            get => _worldState;
        }
    }
}