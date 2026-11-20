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
            if (GameManager.Instance.CurrentState == GameState.Playing)
                GameManager.Instance.ChangeState(GameState.Pause);
            else
                GameManager.Instance.ChangeState(GameState.Playing);

            Debug.Log($"상태: {GameManager.Instance.CurrentState}");
        }
    }
}
