using UnityEditor.Search;
using UnityEngine;

public class FloatingOrb : MonoBehaviour
{
    [Header("타겟")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0.7f, 1.6f, 0.3f);

    [Header("효과")]
    [SerializeField] private float followSmoothTime = 0.05f;
    [SerializeField] private float bobHeight = 0.15f;
    [SerializeField] private float bobSpeed = 2f;

    private Vector3 followVelocity;

    void Start()
    {
        transform.SetParent(null);
        transform.position = GetFollowPoint();
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 destination = GetFollowPoint();
        destination.y += Mathf.Sin(Time.time * bobSpeed) * bobHeight;

        transform.position = Vector3.SmoothDamp(transform.position, destination, ref followVelocity, followSmoothTime);
    }

    private Vector3 GetFollowPoint()
    {
        return target.TransformPoint(offset);
    }
}
