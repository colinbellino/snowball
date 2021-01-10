﻿using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class BattleDefeatState : BaseBattleState
	{
		public BattleDefeatState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public async override UniTask Enter()
		{
			await base.Enter();

			Debug.Log("Battle lost :(");

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}
	}
}
