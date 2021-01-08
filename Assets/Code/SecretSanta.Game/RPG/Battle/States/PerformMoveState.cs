using Cysharp.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class PerformMoveState : BaseBattleState, IState
	{
		public PerformMoveState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public async UniTask Enter(object[] args)
		{
			await _turn.Unit.Facade.MoveOnPath(_turn.MovePath);
			await _turn.Unit.Facade.ChangeDirection(_turn.Unit.Direction.x);

			_turn.Unit.GridPosition = _turn.MovePath[_turn.MovePath.Count - 1];
			_turn.HasMoved = true;

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}

		public UniTask Exit() { return default; }

		public void Tick() { }
	}
}
