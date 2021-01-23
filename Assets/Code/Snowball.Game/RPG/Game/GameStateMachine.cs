﻿using System;
using System.Collections.Generic;
using Stateless;

namespace Snowball.Game
{
	public class GameStateMachine
	{
		public enum States
		{
			Bootstrap,
			Title,
			Battle,
			Worldmap,
			Credits,
		}
		public enum Triggers
		{
			Done,
			StartBattle,
			StartWorldmap,
			StartCredits,
			BackToTitle,
		}

		private readonly Dictionary<States, IState> _states;
		private readonly StateMachine<States, Triggers> _machine;
		private IState _currentState;

		public GameStateMachine(GameConfig config, Worldmap worldmap, GameState state, AudioPlayer audioPlayer)
		{
			_states = new Dictionary<States, IState>
			{
				{ States.Bootstrap, new BootstrapState(this) },
				{ States.Title, new TitleState(this) },
				{ States.Worldmap, new WorldmapState(this, worldmap, config, state, audioPlayer) },
				{ States.Battle, new BattleState(this) },
				{ States.Credits, new CreditsState(this) },
			};

			_machine = new StateMachine<States, Triggers>(States.Bootstrap);
			_machine.OnTransitioned(OnTransitioned);

			_machine.Configure(States.Bootstrap)
				.Permit(Triggers.Done, States.Title);

			_machine.Configure(States.Title)
				.Permit(Triggers.StartBattle, States.Battle)
				.Permit(Triggers.StartWorldmap, States.Worldmap);

			_machine.Configure(States.Worldmap)
				.Permit(Triggers.BackToTitle, States.Title)
				.Permit(Triggers.StartBattle, States.Battle);

			_machine.Configure(States.Battle)
				.Permit(Triggers.StartWorldmap, States.Worldmap)
				.Permit(Triggers.StartCredits, States.Credits);

			_machine.Configure(States.Credits)
				.Permit(Triggers.Done, States.Title);

			_currentState = _states[_machine.State];
		}

		public async void Start()
		{
			await _currentState.Enter();
		}

		public void Tick() => _currentState?.Tick();

		public void Fire(Triggers trigger) => _machine.Fire(trigger);

		private async void OnTransitioned(StateMachine<States, Triggers>.Transition transition)
		{
			if (_currentState != null)
			{
				await _currentState.Exit();
			}

			if (_states.ContainsKey(transition.Destination) == false)
			{
				throw new Exception("Missing state class for: " + transition.Destination);
			}

			_currentState = _states[transition.Destination];
			// Debug.Log($"{source.UnderlyingState} -> {destination.UnderlyingState}");

			await _currentState.Enter();
		}
	}
}
