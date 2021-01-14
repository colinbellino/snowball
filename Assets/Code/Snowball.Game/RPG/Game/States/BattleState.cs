﻿using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Snowball.Game
{
	public class BattleState : IState
	{
		private readonly GameStateMachine _machine;

		private BattleStateMachine _battleMachine;
		private TurnManager _turnManager;

		public BattleState(GameStateMachine machine)
		{
			_machine = machine;
		}

		public async UniTask Enter()
		{
			_turnManager = new TurnManager();
			_battleMachine = new BattleStateMachine(_turnManager);

			_battleMachine.Start();
			_battleMachine.BattleEnded += OnBattleEnded;

			await Game.Instance.Transition.EndTransition(Color.white);
		}

		public async UniTask Exit()
		{
			_battleMachine.BattleEnded -= OnBattleEnded;

			await Game.Instance.Transition.StartTransition(Color.white);
		}

		public void Tick()
		{
			_battleMachine?.Tick();
		}

		private void OnBattleEnded(BattleResults result)
		{
			if (result == BattleResults.Victory)
			{
				Game.Instance.State.EncountersDone.Add(Game.Instance.State.CurrentEncounter);
			}

			_machine.Fire(GameStateMachine.Triggers.StartWorldmap);
		}
	}
}
