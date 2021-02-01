using Cysharp.Threading.Tasks;

namespace Snowball.Game
{
	public class PerformActionState : BaseBattleState
	{
		public PerformActionState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override async UniTask Enter()
		{
			await base.Enter();

			_ui.SetTurnUnit(_turn.Unit);

			await _turn.Plan.Ability.Perform(_turnManager);

			_turn.HasActed = true;

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}

		public override async UniTask Exit()
		{
			await base.Exit();

			_ui.SetTurnUnit(null);
		}

		protected override void OnPause()
		{
			PauseManager.Pause();
		}
	}
}
