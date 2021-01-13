using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Snowball.Tests
{
	public class MyTests : InputTestFixture
	{
	    [UnityTest]
	    public IEnumerator Hello() => UniTask.ToCoroutine(async () =>
	    {
		    var keyboard = InputSystem.AddDevice<Keyboard>();
		    var mouse = InputSystem.AddDevice<Mouse>();

		    await LoadScene("RPG");
		    await UniTask.Delay(500);

		    Debug.Log("F1");
		    Press(keyboard.f1Key);

		    Move(mouse.position, new Vector2(345, 225));
		    PressAndRelease(mouse.leftButton);

		    await UniTask.Delay(1000);

		    Assert.AreEqual(3, 3);
	    });

	    private static async UniTask LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
	    {
		    Debug.Log("Loading scene: " + sceneName);
		    await SceneManager.LoadSceneAsync(sceneName, mode);
	    }
	}
}
