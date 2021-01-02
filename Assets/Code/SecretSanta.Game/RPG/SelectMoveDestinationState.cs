using System.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectMoveDestinationState : BaseBattleState, IState
	{
		public SelectMoveDestinationState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args) { }

		public async Task Exit()
		{
			_ui.ClearMovePath();
		}

		public void Tick()
		{
			const int maxDistance = 5;

			var mousePosition = _controls.Gameplay.MousePosition.ReadValue<Vector2>();
			var leftClick = _controls.Gameplay.LeftClick.ReadValue<float>() > 0f;

			var mouseWorldPosition = _camera.ScreenToWorldPoint(mousePosition);
			var cursorPosition = Helpers.GetCursorPosition(mouseWorldPosition, _config.TilemapSize);

			var path = Helpers.CalculatePathWithFall(
				_turn.Unit.GridPosition, cursorPosition,
				_battle.WalkGrid
			);
			// TODO: Store movePath in Turn instead of destination
			_ui.HighlightMovePath(path);

			if (leftClick)
			{
				if (Helpers.CanMoveTo(cursorPosition, _battle.WalkGrid) && Helpers.IsInRange(_turn.InitialPosition, cursorPosition, maxDistance))
				{
					_turn.MoveDestination = cursorPosition;
					_machine.Fire(BattleStateMachine.Triggers.MoveDestinationSelected);
					return;
				}
			}
		}
	}
}
