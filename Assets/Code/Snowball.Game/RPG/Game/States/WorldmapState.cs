using System.Linq;
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
		private readonly AudioPlayer _audio;
		private ParticleSystem _snowballEffect;

		public WorldmapState(GameStateMachine machine, Worldmap worldmap, GameConfig config, GameState state, AudioPlayer audioPlayer)
		{
			_machine = machine;
			_worldmap = worldmap;
			_config = config;
			_state = state;
			_audio = audioPlayer;
		}

		public async UniTask Enter()
		{
			_snowballEffect = Game.Instance.Spawner.SpawnEffect(Game.Instance.Config.SnowfallPrefab);

			SaveHelpers.SaveToFile(Game.Instance.State);

			_worldmap.Show();
			var availableEncounters = _config.Encounters.Select(id => Game.Instance.Database.Encounters[id]).ToList();
			_worldmap.SetEncounters(availableEncounters, Game.Instance.State);
			_worldmap.EncounterClicked += OnEncounterClicked;

			_audio.PlayMusic(_config.WorldmapMusic, false, 0.5f);
			await Game.Instance.Transition.EndTransition(Color.white);
		}

		public async UniTask Exit()
		{
			_audio.StopMusic();
			await Game.Instance.Transition.StartTransition(Color.white);

			_worldmap.EncounterClicked -= OnEncounterClicked;
			_worldmap.Hide();

			GameObject.Destroy(_snowballEffect.gameObject);
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
			_state.CurrentEncounter = encounterId;

			_machine.Fire(GameStateMachine.Triggers.StartBattle);
		}
	}
}
