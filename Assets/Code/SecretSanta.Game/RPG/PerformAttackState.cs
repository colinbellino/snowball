using System.Linq;
using System.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class PerformAttackState : BaseBattleState, IState
	{
		public PerformAttackState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args)
		{
			var targets = _turn.Targets
				.Select(position => _allUnits.Find(unit => unit.GridPosition == position))
				.Where(unit => unit != null)
				.ToList();
			var result = new AttackResult
			{
				Attacker = _turn.Unit,
				Targets = targets,
				Destination = _turn.Targets[0], // TODO: Calculate where this actually hits
			};
			await _turn.Unit.Attack(result);
			_turn.HasActed = true;

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
