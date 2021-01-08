using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectAttackTargetState : BaseBattleState, IState
	{
		private Vector3Int _cursorPosition;
		public SelectAttackTargetState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public async UniTask Enter(object[] args)
		{
			_ui.InitActionMenu(_turn.Unit);

			if (_turn.Unit.IsPlayerControlled == false)
			{
				await UniTask.Delay(300);
				_machine.Fire(BattleStateMachine.Triggers.TargetSelected);
			}
		}

		public UniTask Exit()
		{
			_ui.InitActionMenu(null);
			_ui.ClearAimPath();

			return default;
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
				var cursorPosition = GridHelpers.GetCursorPosition(mouseWorldPosition, _config.TilemapSize);
				if (cursorPosition != _cursorPosition)
				{
					_cursorPosition = cursorPosition;

					var hitChance = GridHelpers.GetHitAccuracy(_turn.Unit.GridPosition, cursorPosition, _turnManager.BlockGrid, _turn.Unit);
					_ui.HighlightAimPath(_turn.Unit.GridPosition, cursorPosition, hitChance);
				}

				if (leftClick)
				{
					_turn.AttackTargets = new List<Vector3Int> { destination };
					// TODO: Does this need to be a path ?
					_turn.AttackPath = new List<Vector3Int>{ _turn.Unit.GridPosition, _cursorPosition };;
					_machine.Fire(BattleStateMachine.Triggers.TargetSelected);
				}
			}
		}
	}
}
