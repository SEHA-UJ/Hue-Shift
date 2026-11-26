using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Hue Shift/Scene Catalog")]
public class SceneCatalog : ScriptableObject
{
    [Header("로비")]
    [SerializeField] private string lobbyScene;

    [Header("스테이지")]
    [SerializeField] private string[] stageScenes;

    public string LobbyScene { get { return lobbyScene; } }

    // 로비 시작버튼이 사용
    public string FirstStage
    {
        get
        {
            if (stageScenes == null || stageScenes.Length == 0)
                return "";
            
            return stageScenes[0];
        }
    }

    public bool TryGetNextStage(string currentScene, out string nextScene)
    {
        nextScene = "";

        for (int i = 0; i < stageScenes.Length; i++)
        {
            if (stageScenes[i] != currentScene)
                continue;
            
            bool isLastStage = i == stageScenes.Length - 1;
            if (isLastStage)
                return false;

            nextScene = stageScenes[i + 1];
            return true;
        }

        Debug.LogWarning($"SceneCatalog: '{currentScene}' 씬이 스테이지 목록에 없습니다.", this);
        return false;
    }

#if UNITY_EDITOR
    // 인스펙터에서 값을 바꾸는 순간 검사 씬 이름 오타를 Play 전에 바로 알 수 있게.
    // (#if UNITY_EDITOR: 에디터 전용 코드라 빌드에 포함 x)\
    void OnValidate()
    {
        WarnIfNotInBuild(lobbyScene);

        if (stageScenes == null)
            return;
        
        for (int i = 0; i < stageScenes.Length; i++)
            WarnIfNotInBuild(stageScenes[i]);

    }

    private void WarnIfNotInBuild(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return;

        UnityEditor.EditorBuildSettingsScene[] buildScenes = UnityEditor.EditorBuildSettings.scenes;

        for (int i = 0; i <buildScenes.Length; i++)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(buildScenes[i].path);
            if (fileName == sceneName && buildScenes[i].enabled) return;
        }

        Debug.LogWarning($"SceneCatalog: '{sceneName}' 씬이 Build Profiles 씬 목록에 없거나 꺼져 있습니다.", this);
    }
#endif
}
