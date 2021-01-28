using UnityEngine;
using UnityEngine.InputSystem;

namespace Snowball.Game
{
	public class GameManager : MonoBehaviour
	{
		private GameStateMachine _stateMachine;

		private void Start()
		{
			Game.Init();

			Cursor.SetCursor(Game.Instance.Config.MouseCursor, Vector2.zero, CursorMode.Auto);

			_stateMachine = new GameStateMachine(GameConfig.GetDebug(Game.Instance.Config.DebugFSM), Game.Instance.Config, Game.Instance.Worldmap, Game.Instance.State, Game.Instance.AudioPlayer);
			_stateMachine.Start();

			Game.Instance.Controls.Global.Enable();
		}

		private void Update()
		{
			Game.Instance?.AudioPlayer.Tick();
			_stateMachine?.Tick();

			#if UNITY_EDITOR
			if (Keyboard.current.f11Key.wasPressedThisFrame)
			{
				Time.timeScale = 1;
			}
			if (Keyboard.current.f12Key.wasPressedThisFrame)
			{
				Time.timeScale += 2;
			}
			#endif
		}
	}
}
