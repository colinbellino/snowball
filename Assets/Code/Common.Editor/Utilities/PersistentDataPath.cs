using System.IO;
using UnityEditor;
using UnityEngine;

public class OpenPersistentDataPath : MonoBehaviour
{
	[MenuItem("Tools/Open PersistentDataPath")]
	static void DoSomething()
	{
		var path = Path.Combine(Application.persistentDataPath);
		EditorUtility.RevealInFinder(path);
	}
}
