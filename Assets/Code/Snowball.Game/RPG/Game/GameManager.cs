using UnityEngine;

namespace Snowball.Game
{
	public class GameManager : MonoBehaviour
	{
		private GameStateMachine _stateMachine;

		private void Start()
		{
			Game.Init();

			Cursor.SetCursor(Game.Instance.Config.MouseCursor, Vector2.zero, CursorMode.Auto);

			_stateMachine = new GameStateMachine(Game.Instance.Config, Game.Instance.Worldmap, Game.Instance.State);
			_stateMachine.Start();
		}

		private void Update()
		{
			Game.Instance.AudioPlayer.Tick();
			_stateMachine?.Tick();
		}
	}
}
