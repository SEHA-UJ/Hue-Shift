using System;
using System.Collections;
using UnityEngine;

public class GameManager : SingletonCore<GameManager>
{
    [Header("게임오버")]
    [SerializeField] private float gameOverDelay = 2f;

    private bool isGameOverPending; // 게임오버 판정 중복 반지용
    
    public GameState CurrentState { get; private set; } = GameState.Playing;

    public event Action<GameState> OnStateChanged;

    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        Debug.Log($"[GameManager] 상태 변경: {newState}"); 
        OnStateChanged?.Invoke(newState);
    }

    public void GameOver()
    {
        if (isGameOverPending || CurrentState == GameState.GameOver) return;

        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        isGameOverPending = true;

        yield return new WaitForSeconds(gameOverDelay);

        ChangeState(GameState.GameOver);
        isGameOverPending = false;
    }

    public void StageClear()
    {
        ChangeState(GameState.GameClear);
    }
}
