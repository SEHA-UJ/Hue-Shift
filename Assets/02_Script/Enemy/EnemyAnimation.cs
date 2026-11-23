using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAnimation : MonoBehaviour
{
    [Header("전환")]
    [SerializeField] private float crossFadeTime = 0.1f;
    [SerializeField] private float speedDampTime  = 0.1f;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");

    private Animator anim;
    private EnemyController controller;
    private NavMeshAgent agent;

    void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<EnemyController>();
        agent = GetComponent<NavMeshAgent>();
    }

    void OnEnable() => controller.OnActionChanged += HandleActionChanged;
    void OnDisable() => controller.OnActionChanged -= HandleActionChanged;

    void Update()
    {
        // 사망시 버그 방지
        if (!agent.enabled) return;

        float speedRatio = agent.velocity.magnitude / agent.speed;
        anim.SetFloat(SpeedHash, speedRatio, speedDampTime, Time.deltaTime);
    }

    private void HandleActionChanged(EnemyAction action)
    {
        string stateName = action.ToString();
        int stateHash = Animator.StringToHash(stateName);

        // 상태 이름이 틀렸거나 아직 안 만들었으면 바로 알 수 있도록 안전 장치
        if (!anim.HasState(0, stateHash))
        {
            Debug.LogWarning($"EnemyAnimation: Animator에 '{stateName}' 상태가 없습니다.", this);
            return;
        }

        // 마지막 0f = 처음부터 재생. 공격 → 추격 → 공격처럼 같은 애니를 다시 틀 때도 처음부터 나오게.
        anim.CrossFade(stateHash, crossFadeTime, 0, 0f);
    }
}
