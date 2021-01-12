using Cysharp.Threading.Tasks;
using UnityEditor;
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

		public UniTask Enter()
		{
			// Do this when we start the game only.
			Game.Instance.LoadStateFromSave();

			if (Game.Instance.Config.SkipTitle)
			{
				StartBattle(0);
				return default;
			}

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
			Game.Instance.State.CurrentEncounterId = encounterId;

			_machine.Fire(GameStateMachine.Triggers.StartBattle);
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
