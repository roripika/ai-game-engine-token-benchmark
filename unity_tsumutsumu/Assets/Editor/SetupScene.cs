using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TsumTsumu.Editor
{
    public static class SetupScene
    {
        public static void CreateAndSetupScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            
            GameObject managerObj = new GameObject("GameManager");
            managerObj.AddComponent<GameManager>();

            EditorSceneManager.SaveScene(scene, "Assets/Scenes/MainScene.unity");
            Debug.Log("Created and saved MainScene.unity with GameManager component");
        }
    }
}
