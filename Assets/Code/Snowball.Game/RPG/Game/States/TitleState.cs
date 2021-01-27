using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
				ResetGameState();
			}

			#if UNITY_EDITOR
			if (Game.Instance.Config.SkipTitle)
			{
				StartBattle(0);
				return;
			}
			#endif

			Game.Instance.TitleUI.Show(hasSaveFile);
			Game.Instance.TitleUI.StartButtonClicked += ContinueGame;
			Game.Instance.TitleUI.NewGameButtonClicked += OnNewGameButtonClicked;
			Game.Instance.TitleUI.QuitButtonClicked += OnQuitButtonClicked;

			Game.Instance.Controls.Global.Pause.performed += OnPausePerformed;

			await Game.Instance.Transition.EndTransition(Color.white);
		}

		public async UniTask Exit()
		{
			Game.Instance.TitleUI.StartButtonClicked -= ContinueGame;
			Game.Instance.TitleUI.NewGameButtonClicked -= OnNewGameButtonClicked;
			Game.Instance.TitleUI.QuitButtonClicked -= OnQuitButtonClicked;

			Game.Instance.Controls.Global.Pause.performed -= OnPausePerformed;

			await Game.Instance.Transition.StartTransition(Color.white);

			Game.Instance.TitleUI.Hide();
		}

		public void Tick()
		{
#if UNITY_EDITOR
			if (Keyboard.current.backquoteKey.wasPressedThisFrame)
			{
				_machine.Fire(GameStateMachine.Triggers.StartWorldmap);
				return;
			}

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

		private void OnPausePerformed(InputAction.CallbackContext obj)
		{
			_machine.Fire(GameStateMachine.Triggers.Quit);
		}

		private void StartBattle(int battleIndex)
		{
			var encounterId = Game.Instance.Config.Encounters[battleIndex];
			Game.Instance.State.CurrentEncounter = encounterId;

			_machine.Fire(GameStateMachine.Triggers.StartBattle);
		}

		private void ContinueGame()
		{
			var introEncounter = Game.Instance.Database.Encounters[Game.Instance.Config.Encounters[0]];
			if (Game.Instance.State.EncountersDone.Contains(introEncounter.Id))
			{
				_machine.Fire(GameStateMachine.Triggers.StartWorldmap);
			}
			else
			{
				Game.Instance.State.CurrentEncounter = Game.Instance.Config.Encounters[0];

				_machine.Fire(GameStateMachine.Triggers.StartBattle);
			}
		}

		private void OnNewGameButtonClicked()
		{
			ResetGameState();
			ContinueGame();
		}

		private void OnQuitButtonClicked()
		{
			_machine.Fire(GameStateMachine.Triggers.Quit);
		}

		private static void ResetGameState()
		{
			Game.Instance.State.CurrentEncounter = -1;
			Game.Instance.State.EncountersDone = new List<int>();
			Game.Instance.State.Party = Game.Instance.Config.StartingParty;
			Debug.Log("Creating save.");
		}
	}
}
