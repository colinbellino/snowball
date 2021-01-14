using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Snowball.Game
{
	public class TitleState : IState
	{
		private readonly GameStateMachine _machine;

		public TitleState(GameStateMachine machine)
		{
			_machine = machine;
		}

		public async UniTask Enter()
		{
			var hasSaveFile = SaveHelpers.HasFile();

			if (hasSaveFile)
			{
				Game.Instance.State.Update(SaveHelpers.LoadFromFile());
			}
			else
			{
				Game.Instance.State.CurrentEncounter = -1;
				Game.Instance.State.EncountersDone = new List<int>();
				Game.Instance.State.Party = Game.Instance.Config.StartingParty;
				Debug.Log("Using save");
			}

			if (Game.Instance.Config.SkipTitle)
			{
				StartBattle(0);
				return;
			}

			Game.Instance.TitleUI.Show(hasSaveFile);
			Game.Instance.TitleUI.StartButtonClicked += OnStartButtonClicked;

			await Game.Instance.Transition.EndTransition(Color.white);
		}

		public async UniTask Exit()
		{
			await Game.Instance.Transition.StartTransition(Color.white);

			Game.Instance.TitleUI.Hide();
			Game.Instance.TitleUI.StartButtonClicked -= OnStartButtonClicked;
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
#endif

			if (Keyboard.current.escapeKey.wasPressedThisFrame)
			{
#if UNITY_EDITOR
				EditorApplication.isPlaying = false;
#else
				UnityEngine.Application.Quit();
#endif
			}
		}

		private void StartBattle(int battleIndex)
		{
			var encounterId = Game.Instance.Config.Encounters[battleIndex];
			Game.Instance.State.CurrentEncounter = encounterId;

			_machine.Fire(GameStateMachine.Triggers.StartBattle);
		}

		private void OnStartButtonClicked()
		{
			_machine.Fire(GameStateMachine.Triggers.StartGame);
		}
	}
}
