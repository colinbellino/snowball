using Cysharp.Threading.Tasks;

namespace Snowball.Game
{
	public class CreditsState : IState
	{
		private readonly GameStateMachine _machine;

		public CreditsState(GameStateMachine machine)
		{
			_machine = machine;
		}

		public async UniTask Enter()
		{
			await UniTask.Delay(3000);

			Game.Instance.CreditsUI.Show();
			Game.Instance.Controls.Credits.Enable();
			Game.Instance.CreditsUI.NewGameClicked += OnNewGameClicked;
			Game.Instance.CreditsUI.QuitClicked += OnQuitClicked;
		}

		public async UniTask Exit()
		{
			Game.Instance.Controls.Credits.Disable();
			Game.Instance.CreditsUI.NewGameClicked -= OnNewGameClicked;
			Game.Instance.CreditsUI.QuitClicked -= OnQuitClicked;

			await Game.Instance.Transition.EndTransition();
			Game.Instance.CreditsUI.Hide();
		}

		public void Tick() { }

		private void OnNewGameClicked()
		{
			Game.Instance.State.Set(TitleState.CreateDefaultState());
			SaveHelpers.SaveToFile(Game.Instance.State);
			_machine.Fire(GameStateMachine.Triggers.BackToTitle);
		}

		private void OnQuitClicked()
		{
			_machine.Fire(GameStateMachine.Triggers.Quit);
		}
	}
}
