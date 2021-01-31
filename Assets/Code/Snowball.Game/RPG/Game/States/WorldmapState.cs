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

			_ = _audio.PlayMusic(_config.WorldmapMusic, false, 0.5f);
			await Game.Instance.Transition.EndTransition();

			Game.Instance.Controls.Global.Pause.performed += OnCancelPerformed;

			Game.Instance.PauseUI.ContinueClicked += OnContinueClicked;
			Game.Instance.PauseUI.QuitClicked += OnQuitClicked;
		}

		public void Tick()
		{
#if UNITY_EDITOR
			if (Keyboard.current.f1Key.wasPressedThisFrame)
			{
				StartBattle(0);
				return;
			}

			if (Keyboard.current.f2Key.wasPressedThisFrame)
			{
				StartBattle(1);
				return;
			}

			if (Keyboard.current.f3Key.wasPressedThisFrame)
			{
				StartBattle(2);
				return;
			}

			if (Keyboard.current.f4Key.wasPressedThisFrame)
			{
				StartBattle(3);
				return;
			}

			if (Keyboard.current.f5Key.wasPressedThisFrame)
			{
				StartBattle(4);
				return;
			}

			if (Keyboard.current.f6Key.wasPressedThisFrame)
			{
				StartBattle(5);
				return;
			}
#endif
		}

		public UniTask Exit()
		{
			Game.Instance.Controls.Global.Pause.performed -= OnCancelPerformed;

			Game.Instance.PauseUI.ContinueClicked -= OnContinueClicked;
			Game.Instance.PauseUI.QuitClicked -= OnQuitClicked;

			_worldmap.EncounterClicked -= OnEncounterClicked;
			_worldmap.Hide();

			GameObject.Destroy(_snowballEffect.gameObject);

			return default;
		}

		private void StartBattle(int battleIndex)
		{
			var encounterId = Game.Instance.Config.Encounters[battleIndex];
			Game.Instance.State.CurrentEncounterId = encounterId;

			_machine.Fire(GameStateMachine.Triggers.StartBattle);
		}

		private async void OnEncounterClicked(int encounterIndex)
		{
			var encounterId = _config.Encounters[encounterIndex];

			if (_state.EncountersDone.Contains(encounterId))
			{
				_ = Game.Instance.AudioPlayer.PlaySoundEffect(Game.Instance.Config.MenuErrorClip);
				return;
			}

			_ = Game.Instance.AudioPlayer.PlaySoundEffect(Game.Instance.Config.MenuConfirmClip);
			_state.CurrentEncounterId = encounterId;

			_ = _audio.StopMusic();

			Game.Instance.Transition.SetText(Game.Instance.Database.Encounters[encounterId].Name, Color.white);
			await Game.Instance.Transition.StartTransition(Color.black, true);

			_machine.Fire(GameStateMachine.Triggers.StartBattle);
		}

		private void OnCancelPerformed(InputAction.CallbackContext obj)
		{
			Game.Instance.PauseManager.Pause();
		}

		private void OnContinueClicked()
		{
			Game.Instance.PauseManager.Resume();
		}

		private void OnQuitClicked()
		{
			_machine.Fire(GameStateMachine.Triggers.Quit);
		}
	}
}
