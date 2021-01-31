using System;
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
			Quit,
		}
		public enum Triggers
		{
			Done,
			StartBattle,
			StartWorldmap,
			StartCredits,
			BackToTitle,
			Quit,
		}

		private readonly Dictionary<States, IState> _states;
		private readonly StateMachine<States, Triggers> _machine;
		private IState _currentState;
		private bool _debug;

		public GameStateMachine(bool debug, GameConfig config, Worldmap worldmap, GameState state, AudioPlayer audioPlayer)
		{
			_debug = debug;
			_states = new Dictionary<States, IState>
			{
				{ States.Bootstrap, new BootstrapState(this) },
				{ States.Title, new TitleState(this) },
				{ States.Worldmap, new WorldmapState(this, worldmap, config, state, audioPlayer) },
				{ States.Battle, new BattleState(this) },
				{ States.Credits, new CreditsState(this) },
				{ States.Quit, new QuitState(this) },
			};

			_machine = new StateMachine<States, Triggers>(States.Bootstrap);
			_machine.OnTransitioned(OnTransitioned);

			_machine.Configure(States.Bootstrap)
				.Permit(Triggers.Done, States.Title);

			_machine.Configure(States.Title)
				.Permit(Triggers.StartBattle, States.Battle)
				.Permit(Triggers.StartWorldmap, States.Worldmap)
				.Permit(Triggers.Quit, States.Quit);

			_machine.Configure(States.Worldmap)
				.Permit(Triggers.BackToTitle, States.Title)
				.Permit(Triggers.StartBattle, States.Battle)
				.Permit(Triggers.Quit, States.Quit);

			_machine.Configure(States.Battle)
				.Permit(Triggers.StartWorldmap, States.Worldmap)
				.Permit(Triggers.StartCredits, States.Credits)
				.Permit(Triggers.Quit, States.Quit);

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

			if (_debug)
			{
				if (_states.ContainsKey(transition.Destination) == false)
				{
					UnityEngine.Debug.LogError("Missing state class for: " + transition.Destination);
				}
			}

			_currentState = _states[transition.Destination];
			if (_debug)
			{
				UnityEngine.Debug.Log($"{transition.Source} -> {transition.Destination}");
			}

			await _currentState.Enter();
		}
	}
}
