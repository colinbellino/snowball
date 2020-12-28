using System.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class PerformMoveState : BaseBattleState, IState
	{
		public PerformMoveState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args)
		{
			var direction = _turn.MoveDestination - _turn.InitialPosition;
			if (direction.y == 0 && direction.x != 0f && direction.x != _turn.Unit.Direction.x)
			{
				await _turn.Unit.Turn(direction);
			}

			var path = Helpers.CalculatePathWithFall(
				_turn.Unit.GridPosition,
				_turn.MoveDestination,
				_tilemap
			);
			await _turn.Unit.MoveOnPath(path);
			_turn.HasMoved = true;

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
