using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.SecretSanta.Game.RPG
{
	public class WorldmapState : IState
	{
		private readonly GameStateMachine _machine;
		private readonly Worldmap _worldmap;
		private readonly GameConfig _config;
		private readonly GameState _state;

		private Unit _leader;

		public WorldmapState(GameStateMachine machine, Worldmap worldmap, GameConfig config, GameState state)
		{
			_machine = machine;
			_worldmap = worldmap;
			_config = config;
			_state = state;
		}

		public async Task Enter(object[] args)
		{
			_worldmap.ShowWorldmap();
			_worldmap.SetEncounters(_config.Encounters);
			_worldmap.EncounterClicked += OnEncounterClicked;

			_leader = _state.Party[0];
			_leader.SetFacade(UnitHelpers.SpawnUnitFacade(_config.UnitWorldmapPrefab, _leader, _config.WorldmapStart, true));
		}

		public async Task Exit()
		{
			_worldmap.EncounterClicked -= OnEncounterClicked;
			_worldmap.HideWorldmap();

			_leader.DestroyFacade();
		}

		public void Tick()
		{
			#if UNITY_EDITOR
			if (Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				_machine.Fire(GameStateMachine.Triggers.BackToTitle);
			}
			#endif
		}

		private void OnEncounterClicked(int encounterIndex)
		{
			var encounterId = _config.Encounters[encounterIndex];
			_state.CurrentEncounterId = encounterId;

			_machine.Fire(GameStateMachine.Triggers.StartBattle);
		}
	}
}
