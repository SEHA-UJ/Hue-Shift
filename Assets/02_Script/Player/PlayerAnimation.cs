using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerState state;
    [SerializeField] private PlayerAttack attack;
    
    [Header("이동 블렌드")]
    [SerializeField] private float moveDampTime = 0.1f;
    
    private bool isDead;
    
    private Animator anim;

    // 파라미터 이름
    private static readonly int MoveXHash = Animator.StringToHash("MoveX");
    private static readonly int MoveYHash = Animator.StringToHash("MoveY");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int DieHash = Animator.StringToHash("Die");

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        controller.OnJumped += PlayJumpAnim;
        attack.OnAttacked += PlayAttackAnim;
        state.OnDied += PlayDieAnim;
    }

    void OnDisable()
    {
        controller.OnJumped -= PlayJumpAnim;
        attack.OnAttacked -= PlayAttackAnim;
        state.OnDied -= PlayDieAnim;
    }

    void Update()
    {
        if (isDead) return;
        MoveBlend();
    }

    private void MoveBlend()
    {
        Vector2 blend = Vector2.ClampMagnitude(inputHandler.MoveInput, 1f);

        anim.SetFloat(MoveXHash, blend.x, moveDampTime, Time.deltaTime);
        anim.SetFloat(MoveYHash, blend.y, moveDampTime, Time.deltaTime);
        anim.SetBool(IsGroundedHash, controller.IsGrounded);
    }

    private void PlayJumpAnim() => anim.SetTrigger(JumpHash);

    private void PlayAttackAnim() => anim.SetTrigger(AttackHash);

    private void PlayDieAnim()
    {
        isDead = true;

        anim.SetFloat(MoveXHash, 0f);
        anim.SetFloat(MoveYHash, 0f);
        anim.SetTrigger(DieHash);
    }
}
