using System.Collections;
using UnityEngine;

public static class GameFlow
{
    public static bool IsPlaying
    {
        get
        {
            if (GameManager.Instance == null) 
                return true;

            return GameManager.Instance.CurrentState == GameState.Playing;
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
