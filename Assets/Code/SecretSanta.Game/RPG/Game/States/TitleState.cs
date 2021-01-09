using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;

namespace Code.SecretSanta.Game.RPG
{
	public class TitleState : IState
	{
		private readonly GameStateMachine _machine;

		public TitleState(GameStateMachine machine)
		{
			_machine = machine;
		}

		public UniTask Enter()
		{
			// Do this when we start the game only.
			Game.Instance.LoadStateFromSave();

			Game.Instance.DebugUI.Show();

			Game.Instance.DebugUI.OnDebugButtonClicked += OnDebugButtonClicked;

			return default;
		}

		public UniTask Exit()
		{
			Game.Instance.DebugUI.Hide();

			Game.Instance.DebugUI.OnDebugButtonClicked -= OnDebugButtonClicked;

			return default;
		}

		public void Tick()
		{
#if UNITY_EDITOR
			if (Keyboard.current.f1Key.wasPressedThisFrame)
			{
				var encounterId = Game.Instance.Config.Encounters[0];
				Game.Instance.State.CurrentEncounterId = encounterId;

				_machine.Fire(GameStateMachine.Triggers.StartBattle);
				return;
			}

			if (Keyboard.current.f2Key.wasPressedThisFrame)
			{
				var encounterId = Game.Instance.Config.Encounters[1];
				Game.Instance.State.CurrentEncounterId = encounterId;

				_machine.Fire(GameStateMachine.Triggers.StartBattle);
				return;
			}
#endif

			if (Keyboard.current.escapeKey.wasPressedThisFrame)
			{
#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#else
					UnityEngine.Application.Quit();
#endif
			}
		}

		private void OnDebugButtonClicked(int key)
		{
			switch (key)
			{
				case 1:
					_machine.Fire(GameStateMachine.Triggers.StartWorldmap);
					return;
			}
		}
	}
}
