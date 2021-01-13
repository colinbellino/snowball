using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Snowball.Game
{
	public class WorldmapState : IState
	{
		private readonly GameStateMachine _machine;
		private readonly Worldmap _worldmap;
		private readonly GameConfig _config;
		private readonly GameState _state;

		public WorldmapState(GameStateMachine machine, Worldmap worldmap, GameConfig config, GameState state)
		{
			_machine = machine;
			_worldmap = worldmap;
			_config = config;
			_state = state;
		}

		public async UniTask Enter()
		{
			_worldmap.ShowWorldmap();
			_worldmap.SetEncounters(_config.Encounters);
			_worldmap.EncounterClicked += OnEncounterClicked;

			await Game.Instance.Transition.EndTransition(Color.white);
		}

		public async UniTask Exit()
		{
			await Game.Instance.Transition.StartTransition(Color.white);

			_worldmap.EncounterClicked -= OnEncounterClicked;
			_worldmap.HideWorldmap();
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
