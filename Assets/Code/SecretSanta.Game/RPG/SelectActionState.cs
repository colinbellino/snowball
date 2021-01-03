using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public enum BattleAction { Attack, Move, Wait }

	public class SelectActionState : BaseBattleState, IState
	{
		public SelectActionState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args)
		{
			_ui.OnActionClicked += OnActionClicked;

			if (_turn.HasActed && _turn.HasMoved)
			{
				_machine.Fire(BattleStateMachine.Triggers.ActionWaitSelected);
				return;
			}

			if (_turn.Unit.IsPlayerControlled)
			{
				_ui.ToggleButton(BattleAction.Move, _turn.HasMoved == false);
				_ui.ToggleButton(BattleAction.Attack, _turn.HasActed == false);
				_ui.ShowActions();
			}
			else
			{
				ComputerTurn();
			}
		}

		public async Task Exit()
		{
			_ui.HideActions();
			_ui.OnActionClicked -= OnActionClicked;
		}

		public void Tick() { }

		private void OnActionClicked(BattleAction action)
		{
			switch (action)
			{
				case BattleAction.Attack:
					_machine.Fire(BattleStateMachine.Triggers.ActionAttackSelected);
					return;
				case BattleAction.Move:
					_machine.Fire(BattleStateMachine.Triggers.ActionMoveSelected);
					return;
				case BattleAction.Wait:
					_machine.Fire(BattleStateMachine.Triggers.ActionWaitSelected);
					return;
			}
		}

		private void ComputerTurn()
		{
			var randomTarget = _allUnits[0];
			_turn.Targets = new List<Vector3Int> { randomTarget.GridPosition };
			_turn.HasActed = true;
			_turn.HasMoved = true;

			_machine.Fire(BattleStateMachine.Triggers.ActionAttackSelected);
		}
	}
}
