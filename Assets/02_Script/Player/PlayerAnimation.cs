using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerAttack))]
public class PlayerAnimation : MonoBehaviour
{
    [Header("이동 블렌드")]
    [SerializeField] private float moveDampTime = 0.1f;
    [SerializeField] private float moveThreshold = 0.1f; // 입력 크기가 이보다 크면 움직이는 중
    
    private PlayerInputHandler inputHandler;
    private PlayerController controller;
    private PlayerState state;
    private PlayerAttack attack;

    private Animator anim;

    private bool isDead;

    // 파라미터 이름
    private static readonly int MoveXHash = Animator.StringToHash("MoveX");
    private static readonly int MoveYHash = Animator.StringToHash("MoveY");
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int StandAttackHash = Animator.StringToHash("StandAttack");
    private static readonly int MoveAttackHash = Animator.StringToHash("MoveAttack");
    private static readonly int DieHash = Animator.StringToHash("Die");

    void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        controller = GetComponent<PlayerController>();
        state = GetComponent<PlayerState>();
        attack = GetComponent<PlayerAttack>();
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
        Vector2 move = inputHandler.MoveInput;
        
        anim.SetFloat(MoveXHash, move.x, moveDampTime, Time.deltaTime);
        anim.SetFloat(MoveYHash, move.y, moveDampTime, Time.deltaTime);

        anim.SetBool(IsMovingHash, IsMoving());
        anim.SetBool(IsGroundedHash, controller.IsGrounded);
    }

    private bool IsMoving()
    {
        Vector2 move = inputHandler.MoveInput;
        return move.sqrMagnitude > moveThreshold * moveThreshold;
    }

    private void PlayJumpAnim() => anim.SetTrigger(JumpHash);

    private void PlayAttackAnim() => anim.SetTrigger(IsMoving() ? MoveAttackHash : StandAttackHash);

    private void PlayDieAnim()
    {
        isDead = true;

        anim.SetFloat(MoveXHash, 0f);
        anim.SetFloat(MoveYHash, 0f);
        anim.SetTrigger(DieHash);
    }
}
