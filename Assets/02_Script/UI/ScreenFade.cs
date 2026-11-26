using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float FadeDuration = 0.5f;

    void Awake()
    {
        SetAlpha(0f);
        fadeImage.gameObject.SetActive(false);
    }

    // 화면 덮기 (투명 -> 검정)
    public IEnumerator FadeOut()
    {
        yield return Fade(0f, 1f);
    }

    // 화면 걷어내기 (검정 -> 투명)
    public IEnumerator FadeIn()
    {
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float from, float to)
    {
        fadeImage.gameObject.SetActive(true);

        float elapsed = 0f;
        while (elapsed < FadeDuration)
        {
            elapsed += Time.unscaledDeltaTime; // 일시정지 중에도 ui가 작동 될 수 있게
            SetAlpha(Mathf.Lerp(from, to, elapsed / FadeDuration));
            yield return null;
        }

        SetAlpha(to);

        if (to <= 0f)
            fadeImage.gameObject.SetActive(false);
    }

    private void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}
