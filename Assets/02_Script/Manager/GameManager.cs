using System;
using UnityEngine;

public class GameManager : SingletonCore<GameManager>
{
    public GamePhase CurrentPhase { get; private set; } = GamePhase.Lobby;

    public event Action <GamePhase> OnPhaseChanged;

    public void SetPhase(GamePhase newPhase)
    {
        if (CurrentPhase == newPhase ) return;

        CurrentPhase = newPhase;
        Debug.Log($"[GameManager] Phase 변경: {newPhase}");
        OnPhaseChanged?.Invoke(newPhase);
    }
}
