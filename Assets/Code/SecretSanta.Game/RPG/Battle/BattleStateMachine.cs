using System;
using System.Collections.Generic;
using System.Linq;
using Stateless;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
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
			SelectAttackTarget,
			PerformAttack,
			EndTurn,
			BattleVictory,
			BattleDefeat,
			EndBattle,
		}
		public enum Triggers {
			BattleStarted,
			UnitSelected,
			ActionMoveSelected,
			MoveDestinationSelected,
			ActionAttackSelected,
			TargetSelected,
			ActionWaitSelected,
			Done,
			Victory,
			Loss,
		}

		private readonly Dictionary<States, IState> _states;
		private readonly StateMachine<States, Triggers> _machine;
		private IState _currentState;

		public event Action BattleOver;

		public BattleStateMachine(TurnManager turnManager)
		{
			_states = new Dictionary<States, IState>
			{
				{ States.StartBattle, new StartBattleState(this, turnManager) },
				{ States.SelectUnit, new SelectUnitState(this, turnManager) },
				{ States.SelectAction, new SelectActionState(this, turnManager) },
				{ States.SelectMoveDestination, new SelectMoveDestinationState(this, turnManager) },
				{ States.PerformMove, new PerformMoveState(this, turnManager) },
				{ States.SelectAttackTarget, new SelectAttackTargetState(this, turnManager) },
				{ States.PerformAttack, new PerformAttackState(this, turnManager) },
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
				.Permit(Triggers.ActionMoveSelected, States.SelectMoveDestination)
				.Permit(Triggers.ActionAttackSelected, States.SelectAttackTarget)
				.Permit(Triggers.ActionWaitSelected, States.EndTurn)
				.Permit(Triggers.Victory, States.BattleVictory)
				.Permit(Triggers.Loss, States.BattleDefeat);

			_machine.Configure(States.SelectMoveDestination)
				.Permit(Triggers.MoveDestinationSelected, States.PerformMove);

			_machine.Configure(States.PerformMove)
				.Permit(Triggers.Done, States.SelectAction);

			_machine.Configure(States.SelectAttackTarget)
				.Permit(Triggers.TargetSelected, States.PerformAttack);

			_machine.Configure(States.PerformAttack)
				.Permit(Triggers.Done, States.SelectAction);

			_machine.Configure(States.EndTurn)
				.Permit(Triggers.Done, States.SelectUnit);

			_machine.Configure(States.BattleVictory)
				.Permit(Triggers.Done, States.EndBattle);

			_machine.Configure(States.BattleDefeat)
				.Permit(Triggers.Done, States.EndBattle);

			_currentState = _states[_machine.State];
			_currentState.Enter(null);
		}

		public void Tick() => _currentState?.Tick();

		public void Fire(Triggers trigger) => _machine.Fire(trigger);

		public void FireBattleOver() => BattleOver?.Invoke();

		private async void OnTransitioned(StateMachine<States, Triggers>.Transition transition)
		{
			var states = _machine.GetInfo().States.ToList();
			var source = states.Find(state => state.UnderlyingState.Equals(transition.Source));
			var destination = states.Find(state => state.UnderlyingState.Equals(transition.Destination));

			// Debug.LogWarning("Exit!");
			if (_currentState != null)
			{
				await _currentState.Exit();
			}

			if (source.Superstate != null && source.Superstate != destination.Superstate)
			{
				// Debug.LogWarning("Exit PARENT!");
				await _states[(States)source.Superstate.UnderlyingState].Exit();
			}

			if (_states.ContainsKey(transition.Destination) == false)
			{
				Debug.LogError("Missing state class for: " + transition.Destination);
			}

			_currentState = _states[transition.Destination];
			// Debug.Log($"{source.UnderlyingState} -> {destination.UnderlyingState}");

			if (source.Superstate != destination.Superstate && destination.Superstate != null)
			{
				// Debug.LogWarning("Enter PARENT!");
				await _states[(States)destination.Superstate.UnderlyingState].Enter(transition.Parameters);
			}

			await _currentState.Enter(transition.Parameters);
		}
	}
}
