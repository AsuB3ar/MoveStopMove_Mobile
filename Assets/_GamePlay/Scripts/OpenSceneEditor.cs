using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
public static class OpenSceneEditor 
{
    // Start is called before the first frame update
    [MenuItem("Open Scene/Init")]
    static void OpenInit()
    {
        EditorSceneManager.OpenScene("Assets/_GamePlay/Scenes/InitScene/InitScene.unity");
    }

    [MenuItem("Open Scene/Load Start")]
    static void OpenLoadStart()
    {
        EditorSceneManager.OpenScene("Assets/_GamePlay/Scenes/InitScene/LoadStartScene.unity");
    }
    [MenuItem("Open Scene/Standard PVE")]
    static void OpenStandardPVE()
    {
        EditorSceneManager.OpenScene("Assets/_GamePlay/Scenes/GameScene/PveStandardScene.unity");
    }

    [MenuItem("Open Scene/Standard PVP")]
    static void OpenStandardPVP()
    {
        EditorSceneManager.OpenScene("Assets/_GamePlay/Scenes/GameScene/PvpStandardScene.unity");
    } 
}
#endif