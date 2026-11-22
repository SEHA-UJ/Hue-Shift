using System.Collections;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private Transform modelRoot;

    [Header("효과")]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashIntensity = 2f;
    [SerializeField] private float flashDuration = 0.1f;

    private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

    private IDamageable damageable;
    private Renderer[] renderers;
    private MaterialPropertyBlock propertyBlock;
    private Coroutine flashRoutine;

    void Awake()
    {   
        damageable = GetComponent<IDamageable>();
        if (damageable == null)
            Debug.LogError("HitFlash: 같은 오브젝트에 IDamageable(PlayerState, EnemyState 등)이 없습니다.", this);

        renderers = modelRoot.GetComponentsInChildren<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    void OnEnable() => damageable.OnDamaged += HandleDamaged;
    void OnDisable() => damageable.OnDamaged -= HandleDamaged;

    private void HandleDamaged()
    {
        // 번쩍이는 도중에 또 맞으면 처음부터 다시
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        ApplyFlash();
        yield return new WaitForSeconds(flashDuration);
        ClearFlash();

        flashRoutine = null;
    }

    private void ApplyFlash()
    {
        propertyBlock.Clear();
        propertyBlock.SetColor(EmissionColorId, flashColor * flashIntensity);

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].SetPropertyBlock(propertyBlock);
        }
    }

    private void ClearFlash()
    {
        // 덮어쓴 값을 지우면 머티리얼 원래 값(원래 발광 부위 포함)으로 돌아간다
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].SetPropertyBlock(null);
        }
    }
}
