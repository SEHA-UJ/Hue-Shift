using System;

public class GameManager : SingletonCore<GameManager>
{
    public GameState CurrentState { get; private set; } = GameState.Playing;

    public event Action<GameState> OnStateChanged;

    public void ChangeState(GameState newstate)
    {
        if (CurrentState == newstate) return;

        CurrentState = newstate;
        OnStateChanged?.Invoke(newstate);
    }
}
