﻿using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Snowball.Game
{
	public class BattleState : IState
	{
		private readonly GameStateMachine _machine;

		private BattleStateMachine _battleMachine;
		private TurnManager _turnManager;
		private ParticleSystem _snowballEffect;

		public BattleState(GameStateMachine machine)
		{
			_machine = machine;
		}

		public async UniTask Enter()
		{
			_turnManager = new TurnManager();
			_battleMachine = new BattleStateMachine(_turnManager);
			_snowballEffect = Game.Instance.Spawner.SpawnEffect(Game.Instance.Config.SnowfallPrefab);

			_battleMachine.Start();
			_battleMachine.BattleEnded += OnBattleEnded;
		}

		public async UniTask Exit()
		{
			_battleMachine.BattleEnded -= OnBattleEnded;

			GameObject.Destroy(_snowballEffect.gameObject);
		}

		public void Tick()
		{
			_battleMachine?.Tick();
		}

		private void OnBattleEnded(BattleResults result)
		{
			var encounterId = Game.Instance.State.CurrentEncounter;

			Game.Instance.State.CurrentEncounter = -1;

			if (result == BattleResults.Victory)
			{
				Game.Instance.State.EncountersDone.Add(encounterId);

				var lastEncounterId = Game.Instance.Config.Encounters[Game.Instance.Config.Encounters.Count - 1];
				if (encounterId == lastEncounterId)
				{
					_machine.Fire(GameStateMachine.Triggers.StartCredits);
					return;
				}
			}

			_machine.Fire(GameStateMachine.Triggers.StartWorldmap);
		}
	}
}
