using System.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectMoveDestinationState : BaseBattleState, IState
	{
		public SelectMoveDestinationState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args) { }

		public async Task Exit() { }

		public void Tick()
		{
			const int maxDistance = 5;

			var mousePosition = _controls.Gameplay.MousePosition.ReadValue<Vector2>();
			var leftClick = _controls.Gameplay.LeftClick.ReadValue<float>() > 0f;

			var mouseWorldPosition = _camera.ScreenToWorldPoint(mousePosition);
			var destination = new Vector3Int(
				Mathf.RoundToInt(mouseWorldPosition.x),
				Mathf.RoundToInt(mouseWorldPosition.y),
				0
			);

			// TODO: Highlight selected tile and path

			if (leftClick)
			{
				if (Helpers.CanMoveTo(destination, _tilemap) && Helpers.IsInRange(_turn.InitialPosition, destination, maxDistance))
				{
					_turn.MoveDestination = destination;
					_machine.Fire(BattleStateMachine.Triggers.MoveDestinationSelected);
					return;
				}
			}
		}
	}
}
