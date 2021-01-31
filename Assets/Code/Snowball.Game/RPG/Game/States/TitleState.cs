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
				var save = SaveHelpers.LoadFromFile();
				if (save.Version != Application.version)
				{
					Debug.Log("Resetting save file (incompatible version).");
					Game.Instance.State.Set(CreateDefaultState());
					SaveHelpers.SaveToFile(Game.Instance.State);
				}
				else
				{
					Game.Instance.State.Set(save);
				}
			}
			else
			{
				Debug.Log("Resetting save file (file not found).");
				Game.Instance.State.Set(CreateDefaultState());
			}

			// if (GameConfig.GetDebug(Game.Instance.Config.DebugSkipTitle))
			if (true)
			{
				ContinueGame();
				return;
			}

			Game.Instance.TitleUI.Show(hasSaveFile);
			Game.Instance.TitleUI.StartButtonClicked += ContinueGame;
			Game.Instance.TitleUI.NewGameButtonClicked += OnNewGameButtonClicked;
			Game.Instance.TitleUI.QuitButtonClicked += OnQuitButtonClicked;

			Game.Instance.Controls.Global.Pause.performed += OnPausePerformed;

			await Game.Instance.Transition.EndTransition();
		}

		public async UniTask Exit()
		{
			Game.Instance.TitleUI.StartButtonClicked -= ContinueGame;
			Game.Instance.TitleUI.NewGameButtonClicked -= OnNewGameButtonClicked;
			Game.Instance.TitleUI.QuitButtonClicked -= OnQuitButtonClicked;

			Game.Instance.Controls.Global.Pause.performed -= OnPausePerformed;

			await Game.Instance.Transition.StartTransition(Color.black);

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
			Game.Instance.State.CurrentEncounterId = encounterId;

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
				Game.Instance.State.CurrentEncounterId = Game.Instance.Config.Encounters[0];

				_machine.Fire(GameStateMachine.Triggers.StartBattle);
			}
		}

		private void OnNewGameButtonClicked()
		{
			Game.Instance.State.Set(CreateDefaultState());
			ContinueGame();
		}

		private void OnQuitButtonClicked()
		{
			_machine.Fire(GameStateMachine.Triggers.Quit);
		}

		private static GameState CreateDefaultState()
		{
			var state = new GameState();
			state.Version = Application.version;
			state.CurrentEncounterId = -1;
			state.EncountersDone = new List<int>();
			state.Party = Game.Instance.Config.StartingParty;
			state.Guests = new List<int>();

			for (var unitIndex = 0; unitIndex < Game.Instance.Config.StartingParty.Count; unitIndex++)
			{
				if (unitIndex > 0)
				{
					state.Guests.Add(Game.Instance.Config.StartingParty[unitIndex]);
				}
			}

			return state;
		}
	}
}
