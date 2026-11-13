using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private PlayerController player;

    [Header("카메라 설정")]
    [SerializeField] private float height = 3f;  // 발 기준 높이
    [SerializeField] private float distance = 4f;  // 뒤로 떨어진 거리
    [SerializeField] private float fixedPitch = 20;  // 내려다보는 각도

    [Header("벽 충돌")]
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float cameraRadius = 0.25f;
    [SerializeField] private float collisionBuffer = 0.3f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (player == null) return;

        Quaternion rot = Quaternion.Euler(fixedPitch, player.Yaw, 0);

        Vector3 pivot = player.transform.position + Vector3.up * height;
        Vector3 backDir = rot * Vector3.back;

        float finalDistance = distance;
        RaycastHit hit;

        if (Physics.SphereCast(pivot, cameraRadius, backDir, out hit, distance, obstacleMask, QueryTriggerInteraction.Ignore))
            finalDistance = Mathf.Max(hit.distance - collisionBuffer, 0f);

        transform.position = pivot + backDir * finalDistance;
        transform.rotation = rot;
    }
}
