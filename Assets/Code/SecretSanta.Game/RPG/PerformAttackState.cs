using System.Linq;
using System.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class PerformAttackState : BaseBattleState, IState
	{
		public PerformAttackState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args)
		{
			var targets = _turn.AttackTargets
				.Select(position => _allUnits.Find(unit => unit.GridPosition == position))
				.Where(unit => unit != null)
				.ToList();
			var result = new AttackResult
			{
				Attacker = _turn.Unit,
				Targets = targets,
				Path = _turn.AttackPath,
			};
			await _turn.Unit.Attack(result);
			_turn.HasActed = true;

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
