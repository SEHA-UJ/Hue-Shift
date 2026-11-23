using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyState))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private EnemyAttack attack;
    
    [Header("감지 거리")]
    [SerializeField] private float detectRange = 5f; // 공격 인식 거리
    [SerializeField] private float loseRange = 10f; // 추적 포기 거리
    
    [Header("사망")]
    [SerializeField] private float dieDuration = 1f;
    [SerializeField] private GameObject deathEffect; 

    private EnemyState state;

    private NavMeshAgent agent;
    private CapsuleCollider bodyColl;

    private Transform target;
    private EnemyAction currentAction = EnemyAction.Idle;
    private Coroutine actionRoutine;

    public EnemyAction CurrentAction { get { return currentAction; } }

    public event Action<EnemyAction> OnActionChanged;

    void Awake()
    {
        state = GetComponent<EnemyState>();
        
        agent = GetComponent<NavMeshAgent>();
        bodyColl = GetComponent<CapsuleCollider>();
    }

    void OnEnable()
    {
        state.OnDied += HandleDied;
    }

    void OnDisable()
    {
        state.OnDied -= HandleDied;
    }

    void Start()
    {
        if (PlayerState.Current != null)
            target = PlayerState.Current.transform;
        
        ChangeAction(EnemyAction.Idle);
    }

    private void HandleDied() => ChangeAction(EnemyAction.Die);

    private void ChangeAction(EnemyAction newAction)
    {
        if (currentAction == EnemyAction.Die) return;

        if (actionRoutine != null)
        {
            StopCoroutine(actionRoutine);
            actionRoutine = null;
        }

        currentAction = newAction;
        OnActionChanged?.Invoke(newAction);

        switch (currentAction)
        {
            case EnemyAction.Idle:
                actionRoutine = StartCoroutine(IdleRoutine());
                break;
            case EnemyAction.Chase:
                actionRoutine = StartCoroutine(ChaseRoutine());
                break;
            case EnemyAction.Attack:
                actionRoutine = StartCoroutine(AttackRoutine());
                break;
            case EnemyAction.Die:
                actionRoutine = StartCoroutine(DieRoutine());
                break;
        }
    }

    private IEnumerator IdleRoutine()
    {
        agent.isStopped = true;

        while (true)
        {
            if (IsTargetInRange(detectRange))
            {
                ChangeAction(EnemyAction.Chase);
                yield break; // 새 루틴 시작으로 해당 루틴을 끝내기
            }

            yield return null;
        }
    }

    private IEnumerator ChaseRoutine()
    {
        while (true)
        {
            if (!IsTargetInRange(loseRange))
            {
                ChangeAction(EnemyAction.Idle);
                yield break;
            }

            // if - 현재 플레이 중이라면 추적, else - 컷신, 일시정지 시 이동 멈춤
            if (GameFlow.IsPlaying)
            {
                // 사거리 안 + 쿨다운 끝 → 공격으로
                if (attack.CanAttack)
                {
                    ChangeAction(EnemyAction.Attack);
                    yield break;
                }
                
                agent.isStopped = false;
                agent.SetDestination(target.position);
            }
            else
                agent.isStopped = true;

            yield return null;
        }
    }

    private IEnumerator AttackRoutine()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        yield return attack.PerformAttack();

        ChangeAction(EnemyAction.Chase);
    }
    
    private IEnumerator DieRoutine()
    {
        agent.enabled = false;

        if (bodyColl != null)
            bodyColl.enabled = false;

        yield return GameFlow.Wait(dieDuration);

        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private bool IsTargetInRange(float range)
    {
        if (target == null) return false;
        
        return Vector3.Distance(transform.position, target.position) <= range;
    }
}
