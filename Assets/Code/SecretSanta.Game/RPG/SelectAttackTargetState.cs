using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectAttackTargetState : BaseBattleState, IState
	{
		public SelectAttackTargetState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args)
		{
			_ui.SetUnit(_turn.Unit);

			if (_turn.Unit.IsPlayerControlled == false)
			{
				await UniTask.Delay(300);
				_machine.Fire(BattleStateMachine.Triggers.TargetSelected);
			}
		}

		public async Task Exit()
		{
			_ui.SetUnit(null);
			_ui.ClearAimPath();
		}

		public void Tick()
		{
			if (_turn.Unit.IsPlayerControlled)
			{
				var mousePosition = _controls.Gameplay.MousePosition.ReadValue<Vector2>();
				var leftClick = _controls.Gameplay.LeftClick.ReadValue<float>() > 0f;

				var mouseWorldPosition = _camera.ScreenToWorldPoint(mousePosition);
				var destination = new Vector3Int(
					Mathf.RoundToInt(mouseWorldPosition.x),
					Mathf.RoundToInt(mouseWorldPosition.y),
					0
				);
				var cursorPosition = Helpers.GetCursorPosition(mouseWorldPosition, _config.TilemapSize);

				var path = new List<Vector3Int>{ _turn.Unit.GridPosition, cursorPosition };
				_ui.HighlightAimPath(path);

				if (leftClick)
				{
					_turn.AttackTargets = new List<Vector3Int> { destination };
					_turn.AttackPath = path;
					_machine.Fire(BattleStateMachine.Triggers.TargetSelected);
				}
			}
		}
	}
}
