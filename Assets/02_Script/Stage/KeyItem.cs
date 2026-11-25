using System;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public event Action OnCollected;

    private bool isCollected;

    void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        if (other.GetComponentInParent<PlayerState>() == null) return;

        isCollected = true;
        OnCollected?.Invoke();
        Destroy(gameObject);
    }
}
