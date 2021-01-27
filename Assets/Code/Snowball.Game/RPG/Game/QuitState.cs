using Cysharp.Threading.Tasks;
using UnityEditor;

namespace Snowball.Game
{
	public class QuitState : IState
	{
		public QuitState(GameStateMachine gameStateMachine) { }

		public UniTask Enter()
		{
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
			UnityEngine.Application.Quit();
#endif

			return default;
		}

		public UniTask Exit() => default;

		public void Tick() { }
	}
}
