using System.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectMoveDestinationState : IState
	{
		private readonly BattleStateMachine _machine;
		private bool _busy;

		public SelectMoveDestinationState(BattleStateMachine machine)
		{
			_machine = machine;
		}

		public async Task Enter(object[] args) { }

		public async Task Exit() { }

		public async void Tick()
		{
			if (_busy) { return; }

			var turn = Game.Instance.Battle.Turn;
			var tilemap = Game.Instance.Tilemap;
			var unit = turn.Unit;
			var maxDistance = 5;
			var maxClimpHeight = 1;

			var moveInput = Game.Instance.Controls.Gameplay.Move.ReadValue<Vector2>();
			var direction = Helpers.InputToDirection(moveInput);

			if (direction.y == 0 && direction.x != 0f && direction.x != unit.Direction.x)
			{
				_busy = true;
				await unit.Turn(direction);
				_busy = false;
			}
			else
			{
				for (var y = 0; y <= maxClimpHeight; y++)
				{
					var destination = unit.GridPosition + direction + Vector3Int.up * y;
					if (Helpers.IsInRange(turn.InitialPosition, destination, maxDistance) == false)
					{
						break;
					}

					if (Helpers.CanMoveTo(destination, tilemap))
					{
						var path = Helpers.CalculatePathWithFall(unit.GridPosition, destination, tilemap);
						_busy = true;
						await unit.MoveOnPath(path);
						_busy = false;
						turn.HasMoved = true;
						break;
					}
				}
			}

			var confirmInput = Game.Instance.Controls.Gameplay.Confirm.ReadValue<float>() > 0f;
			if (confirmInput)
			{
				_machine.Fire(BattleStateMachine.Triggers.Done);
			}
		}
	}
}
