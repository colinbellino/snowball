using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class SelectAttackTargetState : BaseBattleState, IState
	{
		public SelectAttackTargetState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args)
		{
			if (_turn.Unit.IsPlayerControlled == false)
			{
				_machine.Fire(BattleStateMachine.Triggers.TargetSelected);
			}
		}

		public async Task Exit() { }

		public void Tick()
		{
			var mousePosition = _controls.Gameplay.MousePosition.ReadValue<Vector2>();
			var leftClick = _controls.Gameplay.LeftClick.ReadValue<float>() > 0f;

			var mouseWorldPosition = _camera.ScreenToWorldPoint(mousePosition);
			var destination = new Vector3Int(
				Mathf.RoundToInt(mouseWorldPosition.x),
				Mathf.RoundToInt(mouseWorldPosition.y),
				0
			);

			// TODO: Highlight target and trajectory

			if (leftClick)
			{
				_turn.Targets = new List<Vector3Int> { destination };
				_machine.Fire(BattleStateMachine.Triggers.TargetSelected);
			}
		}
	}
}
