using System;
using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("씬 오브젝트")]
    [SerializeField] private PlayerState player;
    [SerializeField] private KeyItem key;
    [SerializeField] private GoalPoint goal;

    [Header("게임오버")]
    [SerializeField] private float gameOverDelay = 2f;

    private bool hasKey;

    public StageState CurrentState { get; private set; } = StageState.Playing;

    public event Action<StageState> OnStateChanged;

    public static StageManager Current { get; private set; }

    void Awake()
    {
        Current = this;
    }

    void OnDestroy()
    {
        if (Current == this)
            Current = null;
    }

    void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.SetPhase(GamePhase.Stage);
    }

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

    public void ChangeState(StageState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        Debug.Log($"[StageManager] 상태 변경: {newState}");
        OnStateChanged?.Invoke(newState);
    }

    private void HandlePlayerDied()
    {
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(gameOverDelay);
        ChangeState(StageState.GameOver);
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

        ChangeState(StageState.StageClear);
    }
}
