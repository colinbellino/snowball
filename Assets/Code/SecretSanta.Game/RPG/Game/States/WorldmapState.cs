using System.Threading.Tasks;
using UnityEngine.InputSystem;

namespace Code.SecretSanta.Game.RPG
{
	public class WorldmapState : IState
	{
		private readonly GameStateMachine _machine;
		private readonly Board _board;
		private readonly GameConfig _config;
		private readonly Database _database;
		private readonly GameState _state;

		public WorldmapState(GameStateMachine machine, Board board, GameConfig config, Database database, GameState state)
		{
			_machine = machine;
			_board = board;
			_config = config;
			_database = database;
			_state = state;
		}

		public async Task Enter(object[] args)
		{
			_board.ShowWorldmap();
			_board.SetEncounters(_config.Encounters);
			_board.EncounterClicked += OnEncounterClicked;
		}

		public async Task Exit()
		{
			_board.HideWorldmap();
			_board.EncounterClicked -= OnEncounterClicked;
		}

		public void Tick()
		{
			if (Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				_machine.Fire(GameStateMachine.Triggers.BackToTitle);
			}
		}

		private void OnEncounterClicked(int encounterIndex)
		{
			var encounterId = _config.Encounters[encounterIndex];
			_state.CurrentEncounterId = encounterId;

			_machine.Fire(GameStateMachine.Triggers.StartBattle);
		}
	}
}
