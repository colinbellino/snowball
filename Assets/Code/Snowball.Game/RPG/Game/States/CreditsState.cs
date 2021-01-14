using Cysharp.Threading.Tasks;
using UnityEngine;

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
			Game.Instance.CreditsUI.Show();

			await UniTask.Delay(3000);
			await Game.Instance.Transition.EndTransition(Color.white);

			Game.Instance.CreditsUI.Hide();

			_machine.Fire(GameStateMachine.Triggers.Done);
		}

		public UniTask Exit() { return default; }

		public void Tick() { }
	}
}
