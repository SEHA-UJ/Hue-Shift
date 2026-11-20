using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerState))]
public class PlayerAttack : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private Transform shotPoint;

    [Header("투사체")]
    [SerializeField] private ColorSet<GameObject> projectilePrefab;
    [SerializeField] private ProjectileStats projectileStats;

    [Header("공격 조건")]
    [SerializeField] private float attackCooldown = 1.5f;

    private PlayerController controller;
    private PlayerState state;
    private PlayerInputHandler inputHandler;
    
    private float lastAttackTime = -999f;

    public event Action OnAttacked;

    void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        controller = GetComponent<PlayerController>();
        state = GetComponent<PlayerState>();
    }

    void Update()
    {
        // 쿨타운 중 누른 건 쌓지 않고 버리기
        if (!inputHandler.ConsumeAttack()) return;
        if (Time.time - lastAttackTime < attackCooldown ) return;
        if (!controller.IsGrounded) return;

        Shot();
        lastAttackTime = Time.time;
        OnAttacked?.Invoke();
    }

    private void Shot()
    {
        ColorType color = state.CurrentColor;
        GameObject prefab = projectilePrefab.Get(color);

        if (prefab == null)
        {
            Debug.LogWarning("투사체 프리팹이 비었습니다.");
            return;
        }

        Vector3 dir = shotPoint.forward;
        GameObject obj = Instantiate(prefab, shotPoint.position, Quaternion.LookRotation(dir));

        Projectile projectile = obj.GetComponent<Projectile>();
        if (projectile != null)
            projectile.Launch(dir, color, projectileStats);
    }
}
