using System;
using System.Collections.Generic;
using Stateless;

namespace Snowball.Game
{
	public class BattleStateMachine
	{
		public enum States
		{
			StartBattle,
			SelectUnit,
			SelectAction,
			SelectMoveDestination,
			PerformMove,
			SelectActionTarget,
			PerformAction,
			EndTurn,
			BattleVictory,
			BattleDefeat,
			EndBattle,
		}
		public enum Triggers {
			BattleStarted,
			UnitSelected,
			MoveSelected,
			MoveDestinationSelected,
			ActionSelected,
			ActionTargetSelected,
			TurnEnded,
			Cancelled,
			Done,
			Victory,
			Defeat,
		}

		private readonly Dictionary<States, IState> _states;
		private readonly StateMachine<States, Triggers> _machine;
		private IState _currentState;

		public event Action<BattleResults> BattleEnded;

		public BattleStateMachine(TurnManager turnManager)
		{
			_states = new Dictionary<States, IState>
			{
				{ States.StartBattle, new StartBattleState(this, turnManager) },
				{ States.SelectUnit, new SelectUnitState(this, turnManager) },
				{ States.SelectAction, new SelectActionState(this, turnManager) },
				{ States.SelectMoveDestination, new SelectMoveDestinationState(this, turnManager) },
				{ States.PerformMove, new PerformMoveState(this, turnManager) },
				{ States.SelectActionTarget, new SelectActionTargetState(this, turnManager) },
				{ States.PerformAction, new PerformActionState(this, turnManager) },
				{ States.EndTurn, new EndTurnState(this, turnManager) },
				{ States.BattleVictory, new BattleVictoryState(this, turnManager) },
				{ States.BattleDefeat, new BattleDefeatState(this, turnManager) },
				{ States.EndBattle, new EndBattleState(this, turnManager) },
			};

			_machine = new StateMachine<States, Triggers>(States.StartBattle);
			_machine.OnTransitioned(OnTransitioned);

			_machine.Configure(States.StartBattle)
				.Permit(Triggers.BattleStarted, States.SelectUnit);

			_machine.Configure(States.SelectUnit)
				.Permit(Triggers.UnitSelected, States.SelectAction);

			_machine.Configure(States.SelectAction)
				.Permit(Triggers.MoveSelected, States.SelectMoveDestination)
				.Permit(Triggers.ActionSelected, States.SelectActionTarget)
				.Permit(Triggers.TurnEnded, States.EndTurn)
				.Permit(Triggers.Victory, States.BattleVictory)
				.Permit(Triggers.Defeat, States.BattleDefeat);

			_machine.Configure(States.SelectMoveDestination)
				.Permit(Triggers.Cancelled, States.SelectAction)
				.Permit(Triggers.MoveDestinationSelected, States.PerformMove);

			_machine.Configure(States.PerformMove)
				.Permit(Triggers.Done, States.SelectAction);

			_machine.Configure(States.SelectActionTarget)
				.Permit(Triggers.Cancelled, States.SelectAction)
				.Permit(Triggers.ActionTargetSelected, States.PerformAction);

			_machine.Configure(States.PerformAction)
				.Permit(Triggers.Done, States.SelectAction);

			_machine.Configure(States.EndTurn)
				.Permit(Triggers.Done, States.SelectUnit);

			_machine.Configure(States.BattleVictory)
				.Permit(Triggers.Done, States.EndBattle);

			_machine.Configure(States.BattleDefeat)
				.Permit(Triggers.Done, States.EndBattle);

			_currentState = _states[_machine.State];
		}

		public async void Start()
		{
			await _currentState.Enter();
		}

		public void Tick() => _currentState?.Tick();

		public void Fire(Triggers trigger) => _machine.Fire(trigger);

		public void FireBattleEnded(BattleResults result) => BattleEnded?.Invoke(result);

		private async void OnTransitioned(StateMachine<States, Triggers>.Transition transition)
		{
			if (_currentState != null)
			{
				await _currentState.Exit();
			}

			if (_states.ContainsKey(transition.Destination) == false)
			{
				UnityEngine.Debug.LogError("Missing state class for: " + transition.Destination);
			}

			_currentState = _states[transition.Destination];
			// UnityEngine.Debug.Log($"{transition.Source} -> {transition.Destination}");

			await _currentState.Enter();
		}
	}
}
