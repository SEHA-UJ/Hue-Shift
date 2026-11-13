using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private PlayerInputHandler inputHandler;

    [Header("이동 및 시점")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float lookSeneitivity = 0.2f; // 감도

    [Header("점프")]
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private float checkeRadius = 0.2f;
    [SerializeField] private Transform checkGround;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;

    private float yaw;
    public float Yaw { get { return yaw; } }

    private bool isGrounded;
    public bool IsGrounded { get { return isGrounded; } }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        yaw = transform.eulerAngles.y;
    }

    void Update()
    {
        yaw += inputHandler.LookInput.x * lookSeneitivity;
    }

    void FixedUpdate()
    {
        CheckGround();
        Rotate();
        Move();
        Jump();
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(checkGround.position, checkeRadius, groundLayer);
    }

    private void Rotate()
    {
        rb.MoveRotation(Quaternion.Euler(0f, yaw, 0f));
    }

    private void Move()
    {
        Vector2 input = inputHandler.MoveInput;

        if (input.sqrMagnitude < 0.01f)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }

        // 바라보는 방향 기준으로 이동 방향 전환
        Vector3 inputDir = new Vector3(input.x, 0f, input.y);
        Vector3 moveDir = Quaternion.Euler(0f, yaw, 0f) * inputDir;

        moveDir = Vector3.ClampMagnitude(moveDir, 1f);

        // 수평 속도만 바꾸고 수직 속도는 물리 그대로
        Vector3 velocity = rb.linearVelocity;
        velocity.x = moveDir.x * moveSpeed;
        velocity.z = moveDir.z * moveSpeed;

        rb.linearVelocity = velocity;
    }

    private void Jump()
    {
        // 이중에서 누른 점프가 쌓이지 않도록 방지
        if (!inputHandler.ConsumeJump())
            return;

        if (!isGrounded)
            return;

        // 떨어지던 속도가 남아 점프 높이를 다르게 하는 걸 방지
        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0f;
        rb.linearVelocity = velocity;

        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

}
