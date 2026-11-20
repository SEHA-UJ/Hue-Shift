using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerState))]
public class PlayerHealthUI : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private Image[] hearts;

    [Header("색")]
    [SerializeField] private Color fullColor = Color.white;
    [SerializeField] private Color emptyColor = new Color(0.25f, 0.25f, 0.25f, 0.6f);

    private PlayerState state;

    void Awake()
    {
        state = GetComponent<PlayerState>();
    }
    
    void OnEnable()
    {
        state.OnHpChanged += UpdateHearths;
    }

    void OnDisable()
    {
        state.OnHpChanged -= UpdateHearths;
    }

    private void UpdateHearths(int currentHp, int maxHp)
    {
        if (hearts.Length < maxHp)
            Debug.LogError($"PlayerHealthUI: 체력 UI({hearts.Length})가 최대 체력({maxHp})보다 적습니다.");

        for (int i  = 0; i < hearts.Length; i++)
        {
            // 최대 체력보다 많은 칸은 숨긴다
            hearts[i].gameObject.SetActive(i < maxHp);
            
            if (i < currentHp)
                hearts[i].color = fullColor;
            else
                hearts[i].color = emptyColor;
        }
        
    }
}
