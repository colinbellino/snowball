using Cysharp.Threading.Tasks;
using static Snowball.Game.UnitHelpers;

namespace Snowball.Game
{
	public class PerformMoveState : BaseBattleState
	{
		public PerformMoveState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override async UniTask Enter()
		{
			await base.Enter();

			var path = GridHelpers.FindPath(
				_turn.Unit.GridPosition, _turn.Plan.MoveDestination,
				_turnManager.WalkGrid
			);

			await MoveOnPath(_turn.Unit.Facade, path);
			_turn.Unit.Direction = VectorToDirection(path[path.Count - 1] - path[path.Count - 2]);
			_turn.Unit.GridPosition = _turn.Plan.MoveDestination;
			_turn.HasMoved = true;

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}

		protected override void OnPause()
		{
			PauseManager.Pause();
		}
	}
}
