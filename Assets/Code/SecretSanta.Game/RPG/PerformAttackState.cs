using System.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class PerformAttackState : BaseBattleState, IState
	{
		public PerformAttackState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args)
		{
			var result = Helpers.CalculateAttackResult(
				_turn.Unit.GridPosition,
				_turn.AttackDirection,
				_allUnits,
				_tilemap
			);
			await _turn.Unit.Attack(result);
			_turn.HasActed = true;

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
