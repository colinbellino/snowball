using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

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

		public UniTask Enter()
		{
			_turnManager = new TurnManager();
			_battleMachine = new BattleStateMachine(_turnManager);
			_snowballEffect = Game.Instance.Spawner.SpawnEffect(Game.Instance.Config.SnowfallPrefab);

			_battleMachine.Start();
			_battleMachine.BattleEnded += OnBattleEnded;

			Game.Instance.PauseUI.ContinueClicked += OnContinueClicked;
			Game.Instance.PauseUI.QuitClicked += OnQuitClicked;

			return default;
		}

		public UniTask Exit()
		{
			_battleMachine.BattleEnded -= OnBattleEnded;

			Game.Instance.PauseUI.ContinueClicked -= OnContinueClicked;
			Game.Instance.PauseUI.QuitClicked -= OnQuitClicked;

			GameObject.Destroy(_snowballEffect.gameObject);

			return default;
		}

		public void Tick()
		{
			_battleMachine?.Tick();
		}

		private void OnBattleEnded(BattleResults result)
		{
			var encounterId = Game.Instance.State.CurrentEncounterId;

			Game.Instance.State.CurrentEncounterId = -1;

			if (result != BattleResults.Defeat)
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

		private void OnContinueClicked()
		{
			Game.Instance.PauseManager.Resume();
		}

		private void OnQuitClicked()
		{
			_machine.Fire(GameStateMachine.Triggers.Quit);
		}
	}
}
