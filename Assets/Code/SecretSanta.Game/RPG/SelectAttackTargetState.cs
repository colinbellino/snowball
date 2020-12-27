using System.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectAttackTargetState : IState
	{
		private readonly BattleStateMachine _machine;

		public SelectAttackTargetState(BattleStateMachine machine)
		{
			_machine = machine;
		}

		public async Task Enter(object[] args)
		{
			var tilemap = Game.Instance.Tilemap;
			var turn = Game.Instance.Battle.Turn;

			var unit = turn.Unit;
			var allUnits = Game.Instance.Battle.Units;

			var result = Helpers.CalculateAttackResult(
				unit.GridPosition,
				turn.AttackDirection,
				allUnits,
				tilemap
			);
			await unit.Attack(result);
			turn.HasActed = true;

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
