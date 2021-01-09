using UnityEngine;

namespace Code.SecretSanta.Game.RPG
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
			_stateMachine?.Tick();
		}
	}
}
