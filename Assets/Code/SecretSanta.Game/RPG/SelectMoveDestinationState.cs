using System.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectMoveDestinationState : BaseBattleState, IState
	{
		public SelectMoveDestinationState(BattleStateMachine machine) : base(machine) { }

		private bool _busy;

		public async Task Enter(object[] args) { }

		public async Task Exit() { }

		public void Tick()
		{
			const int maxDistance = 5;
			const int maxStepHeight = 1;

			var moveInput = _controls.Gameplay.Move.ReadValue<Vector2>();
			if (moveInput.magnitude > 0f)
			{
				var direction = Helpers.InputToDirection(moveInput);

				for (var y = 0; y <= maxStepHeight; y++)
				{
					var destination = _turn.Unit.GridPosition + direction + Vector3Int.up * y;
					if (Helpers.IsInRange(_turn.InitialPosition, destination, maxDistance) == false)
					{
						break;
					}

					if (Helpers.CanMoveTo(destination, _tilemap))
					{
						_turn.MoveDestination = destination;
						_machine.Fire(BattleStateMachine.Triggers.MoveDestinationSelected);
						return;
					}
				}
			}
		}
	}
}
