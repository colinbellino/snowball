using System.Threading.Tasks;
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

		public async Task Enter(object[] args)
		{
			// Do this when we start the game only.
			Game.Instance.LoadStateFromSave();

			Game.Instance.DebugUI.Show();

			Game.Instance.DebugUI.OnDebugButtonClicked += OnDebugButtonClicked;
		}

		public async Task Exit()
		{
			Game.Instance.DebugUI.OnDebugButtonClicked -= OnDebugButtonClicked;
		}

		public void Tick()
		{
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
			Game.Instance.DebugUI.Hide();

			switch (key)
			{
				case 1:
					_machine.Fire(GameStateMachine.Triggers.StartWorldmap);
					return;
			}
		}
	}
}
