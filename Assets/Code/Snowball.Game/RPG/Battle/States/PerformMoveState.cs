using Cysharp.Threading.Tasks;

namespace Snowball.Game
{
	public class PerformMoveState : BaseBattleState
	{
		public PerformMoveState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override async UniTask Enter()
		{
			await base.Enter();

			await _turn.Unit.Facade.MoveOnPath(_turn.MovePath);
			await _turn.Unit.Facade.AnimateChangeDirection(_turn.Unit.Direction);

			_turn.Unit.GridPosition = _turn.MovePath[_turn.MovePath.Count - 1];
			_turn.HasMoved = true;

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}
	}
}
