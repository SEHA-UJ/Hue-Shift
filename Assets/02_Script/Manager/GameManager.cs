using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonCore<GameManager>
{
    [Header("씬 전환")]
    [SerializeField] private ScreenFade fadePrefab;
    
    private ScreenFade fade;
    private bool isLoading;
    private GamePhase pendingPhase;
    
    public GamePhase CurrentPhase { get; private set; } = GamePhase.Lobby;

    public event Action <GamePhase> OnPhaseChanged;

    protected override void Awake()
    {
        base.Awake();

        if (Instance != this) return;

        if (fadePrefab != null)
            fade = Instantiate(fadePrefab, transform);
    }
    
    // 각 씬의 매니저가 시작할 때 해당 씬이 무엇인지 알림.
    public void SetPhase(GamePhase newPhase)
    {
        if (isLoading)
        {
            pendingPhase = newPhase;
            return;
        }
        
        ApplyPhase(newPhase);
    }

    // 실제로 Phase를 바꾸는 곳 (페이드인아웃 중에 씬 변경으로 인한 플레이어/몹/전투 타이머 실행 방지를 위함)
    private void ApplyPhase(GamePhase newPhase)
    {
        if (CurrentPhase == newPhase ) return;

        CurrentPhase = newPhase;
        Debug.Log($"[GameManager] Phase 변경: {newPhase}");
        OnPhaseChanged?.Invoke(newPhase);
    }

    public void LoadScene(string sceneName)
    {
        if (isLoading) return;

        // 오타 또는 빌드 목록 누락일 경우
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"GameManager: '{sceneName}' 씬을 로드할 수 없습니다. 이름과 Build Profiles 씬 목록을 확인하세요.", this);
            return;
        }

        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        isLoading = true;

        // 새 씬이 Phase를 안 알려주면 이전 Phase를 그대로 쓴다
        pendingPhase = CurrentPhase;

        // 전환하는 동안 입력·몹·전투 타이머를 멈춘다 (GameFlow.IsPlaying이 false가 됨)
        ApplyPhase(GamePhase.Loading);

        if (fade != null)
            yield return fade.FadeOut();

        Time.timeScale = 1f;

        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);
        while (!loading.isDone)
            yield return null;

        if (fade != null)
            yield return fade.FadeIn();

        isLoading = false;
        ApplyPhase(pendingPhase);
    }
}
