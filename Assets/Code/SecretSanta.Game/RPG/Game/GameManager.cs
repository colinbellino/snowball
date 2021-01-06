using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class GameManager : MonoBehaviour
	{
		private GameStateMachine _stateMachine;

		private void Start()
		{
			Game.Init();
			_stateMachine = new GameStateMachine(Game.Instance.Board);
		}

		private void Update()
		{
			_stateMachine?.Tick();
		}
	}
}
