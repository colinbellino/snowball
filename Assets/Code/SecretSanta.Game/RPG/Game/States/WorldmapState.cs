using System.Threading.Tasks;
using UnityEngine.InputSystem;

namespace Code.SecretSanta.Game.RPG
{
	public class WorldmapState : IState
	{
		private readonly GameStateMachine _machine;
		private readonly Board _board;
		private readonly GameConfig _config;
		private readonly GameState _state;

		private Unit _leader;

		public WorldmapState(GameStateMachine machine, Board board, GameConfig config, GameState state)
		{
			_machine = machine;
			_board = board;
			_config = config;
			_state = state;
		}

		public async Task Enter(object[] args)
		{
			_board.ShowWorldmap();
			_board.SetEncounters(_config.Encounters);
			_board.EncounterClicked += OnEncounterClicked;

			var _leader = _state.Party[0];
			_leader.SetFacade(UnitHelpers.SpawnUnitFacade(_config.UnitWorldmapPrefab, _leader, _config.WorldmapStart, true));
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
