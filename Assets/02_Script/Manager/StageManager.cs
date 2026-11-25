using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("씬 오브젝트")]
    [SerializeField] private PlayerState player;
    [SerializeField] private KeyItem key;
    [SerializeField] private GoalPoint goal;

    private bool hasKey;

    void OnEnable()
    {
        player.OnDied += HandlePlayerDied;

        if (key != null)
            key.OnCollected += HandleKeyCollected;

        if (goal != null)
            goal.OnPlayerEntered += HandleGoalEntered;

    }

    void OnDisable()
    {
        player.OnDied -= HandlePlayerDied;

        // 열쇠는 먹으면 사라지니까, 이미 없으면 해제할 것도 없다
        if (key != null)
            key.OnCollected -= HandleKeyCollected;

        if (goal != null)
            goal.OnPlayerEntered -= HandleGoalEntered;
    }

    private void HandlePlayerDied()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("StageManager: 씬에 GameManager가 없습니다.", this);
            return;
        }

        GameManager.Instance.GameOver();
    }

    private void HandleKeyCollected()
    {
        hasKey = true;
        Debug.Log("[StageManager] 열쇠 획득");
    }

    private void HandleGoalEntered()
    {
        if (!hasKey)
        {
            // "열쇠가 필요합니다" 안내 UI로 바꿀 자리
            Debug.Log("[StageManager] 열쇠가 필요합니다");
            return;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogWarning("StageManager: 씬에 GameManager가 없습니다.", this);
            return;
        }

        GameManager.Instance.StageClear();
    }
}
