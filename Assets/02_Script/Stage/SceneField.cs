using UnityEngine;

[System.Serializable]
public class SceneField
{
    [SerializeField] private Object sceneAsset;
    [SerializeField] private string sceneName;

    public string SceneName { get { return sceneName; } }

#if UNITY_EDITOR
    public void SyncName()
    {
        if (sceneName == null)
        {
            sceneName = "";
            return;
        }

        //씬이 아닌 다른 걸 넣었다면 비우기
        UnityEditor.SceneAsset scene = sceneAsset as UnityEditor.SceneAsset;
        if (scene == null)
        {
            Debug.LogWarning($"SceneField: '{sceneAsset.name}'은(는) 씬 파일이 아닙니다.");
            sceneAsset = null;
            sceneName = "";
            return;
        }

        sceneName = scene.name;
    }
#endif
}
