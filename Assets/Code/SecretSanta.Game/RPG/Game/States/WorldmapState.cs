using System.Threading.Tasks;
using UnityEngine.InputSystem;

namespace Code.SecretSanta.Game.RPG
{
	public class WorldmapState : IState
	{
		private readonly GameStateMachine _machine;
		private readonly Board _board;

		public WorldmapState(GameStateMachine machine, Board board)
		{
			_machine = machine;
			_board = board;
		}

		public async Task Enter(object[] args)
		{
			_board.ShowWorldmap();
		}

		public async Task Exit()
		{
			_board.HideWorldmap();
		}

		public void Tick()
		{
			if (Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				_machine.Fire(GameStateMachine.Triggers.BackToTitle);
			}
		}
	}
}
