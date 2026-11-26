using UnityEngine;
using UnityEngine.InputSystem;

public class PauseTest : MonoBehaviour
{
    void Update()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (StageManager.Current.CurrentState == StageState.Playing)
                StageManager.Current.ChangeState(StageState.Pause);
            else
                StageManager.Current.ChangeState(StageState.Playing);
        }
    }
}
