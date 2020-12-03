// using UnityEditor.SceneManagement;
// using UnityEngine;
//
// public class LevelAuthoringProcessor : AssetModificationProcessor
// {
// 	public static string[] OnWillSaveAssets(string[] paths)
// 	{
// 		var levelPath = "";
//
// 		foreach(var path in paths)
// 		{
// 			if(path.StartsWith("Assets/Resources/Levels/"))
// 			{
// 				levelPath = path;
// 				break;
// 			}
// 		}
//
// 		if (string.IsNullOrEmpty(levelPath) == false)
// 		{
// 			// var scene = EditorSceneManager.OpenPreviewScene(assetPath) : throw new ArgumentException(string.Format("Path: {0}, is not a prefab file", (object) assetPath));
// 			var scene = EditorSceneManager.GetActiveScene();
// 			// Debug.Log(PrefabStage.scen);
// 			var root = scene.GetRootGameObjects()[0];
// 			Debug.Log(root);
// 			GameObject[] rootGameObjects = scene.GetRootGameObjects();
//
// 			// var contentsRoot = PrefabUtility.LoadPrefabContents(levelPath);
// 			// var level = contentsRoot.GetComponent<LevelAuthoring>();
// 			// level.Save();
// 			// PrefabUtility.SavePrefabAsset(contentsRoot);
// 		}
//
// 		return paths;
// 	}
// }
