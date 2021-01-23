﻿using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Snowball.Game
{
	public class EndBattleState : BaseBattleState
	{
		public EndBattleState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override async UniTask Enter()
		{
			await base.Enter();

			_ui.HideAll();
			_board.ClearArea();
			_board.HideEncounter();

			foreach (var unit in _turnManager.SortedUnits)
			{
				GameObject.Destroy(unit.Facade);
				unit.Facade = null;
			}

			_audio.StopMusic();
			await Game.Instance.Transition.StartTransition(Color.white);

			var result = _turnManager.GetBattleResult();
			_machine.FireBattleEnded(result);
		}
	}
}
