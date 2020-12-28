using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
			BattleWon,
		}

		private Dictionary<States, IState> _states;
		private StateMachine<States, Triggers> _machine;
		private IState _currentState;

		public BattleStateMachine()
		{
			_states = new Dictionary<States, IState>
			{
				{ States.StartBattle, new StartBattleState(this) },
				{ States.SelectUnit, new SelectUnitState(this) },
				{ States.SelectAction, new SelectActionState(this) },
				{ States.SelectMoveDestination, new SelectMoveDestinationState(this) },
				{ States.PerformMove, new PerformMoveState(this) },
				{ States.SelectAttackTarget, new SelectAttackTargetState(this) },
				{ States.PerformAttack, new PerformAttackState(this) },
				{ States.EndTurn, new EndTurnState(this) },
				{ States.EndBattle, new EndBattleState(this) },
			};

			_machine = new StateMachine<States, Triggers>(States.StartBattle);
			_machine.OnTransitioned(OnTransitioned);

			_machine.Configure(States.StartBattle)
				.Permit(Triggers.BattleStarted, States.SelectUnit);

			_machine.Configure(States.SelectUnit)
				.Permit(Triggers.UnitSelected, States.SelectAction)
				.Permit(Triggers.BattleWon, States.EndBattle);

			_machine.Configure(States.SelectAction)
				.Permit(Triggers.ActionMoveSelected, States.SelectMoveDestination)
				.Permit(Triggers.ActionAttackSelected, States.SelectAttackTarget)
				.Permit(Triggers.ActionWaitSelected, States.EndTurn);

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

			_currentState = _states[_machine.State];
			_currentState.Enter(null);
		}

		public void Tick() => _currentState?.Tick();

		public void Fire(Triggers trigger) => _machine.Fire(trigger);

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
			Debug.Log($"{source.UnderlyingState} -> {destination.UnderlyingState}");

			if (source.Superstate != destination.Superstate && destination.Superstate != null)
			{
				// Debug.LogWarning("Enter PARENT!");
				await _states[(States)destination.Superstate.UnderlyingState].Enter(transition.Parameters);
			}

			await _currentState.Enter(transition.Parameters);
		}
	}

	public interface IState
	{
		Task Enter(object[] args);
		Task Exit();
		void Tick();
	}
}
