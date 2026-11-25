using System;
using UnityEngine;

public class GoalPoint : MonoBehaviour
{
    public event Action OnPlayerEntered;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerState>() == null) return;
        
        OnPlayerEntered?.Invoke();
    }
}
