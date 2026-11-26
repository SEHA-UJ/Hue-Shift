using System.Collections;
using UnityEngine;

public static class GameFlow
{
    public static bool IsPlaying
    {
        get
        {
            if (GameManager.Instance != null && GameManager.Instance.CurrentPhase == GamePhase.Loading) 
                return false;

            if (StageManager.Current == null)
                return true;

            return StageManager.Current.CurrentState == StageState.Playing;
        }
    }

    public static IEnumerator Wait(float seconds)
    {
        float elapsed = 0f;

        while (elapsed < seconds)
        {
            if (IsPlaying)
                elapsed += Time.deltaTime;

            yield return null;
        }
    }
}
