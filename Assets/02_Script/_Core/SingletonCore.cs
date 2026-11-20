using UnityEngine;

public class SingletonCore<T> : MonoBehaviour where T : SingletonCore<T>
{
    private static T instance;
    public static T Instance
    {
        get { return instance; }
    }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log($"[{typeof(T).Name}] 이미 Instance가 있어서 중복 오브젝트를 파괴합니다.", this);
            Destroy(gameObject);
            return;
        }

        instance = (T)this;
        
        if (transform.parent != null)
            transform.SetParent(null);

        DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnDestroy()
    {
        // 내가 등록된 인스턴스일 때만 비운다
        if (instance == this)
        {
            instance = null;
        }
    }
}
