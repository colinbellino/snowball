﻿using System.Threading.Tasks;

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

		public void Tick() { }

		private void OnDebugButtonClicked(int key)
		{
			Game.Instance.DebugUI.Hide();

			switch (key)
			{
				case 0:
					_machine.Fire(GameStateMachine.Triggers.StartBattle);
					return;
				case 1:
					_machine.Fire(GameStateMachine.Triggers.StartWorldmap);
					return;
			}
		}
	}
}
