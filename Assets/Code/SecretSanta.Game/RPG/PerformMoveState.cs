using System.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class PerformMoveState : BaseBattleState, IState
	{
		public PerformMoveState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args)
		{
			var destination = _turn.MovePath[_turn.MovePath.Count - 1];
			var direction = destination - _turn.InitialPosition;

			if (direction.y == 0 && direction.x != 0f && direction.x != _turn.Unit.Direction.x)
			{
				await _turn.Unit.Turn(direction);
			}

			await _turn.Unit.MoveOnPath(_turn.MovePath);
			_turn.HasMoved = true;

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
