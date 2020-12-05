using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stateless;
using UnityEngine;

public class GameStateMachine
{
	private enum States { Bootstrap, Title, Recruitment, Gameplay, Victory, GameOver, Quit }
	public enum Triggers { StartTitle, StartRecruitment, StartLevel, FinishLevel, Win, Lose, Quit }

	private StateMachine<States, Triggers> _machine;
	private readonly Dictionary<States, IState> _states;
	private IState _currentState;

	public GameStateMachine()
	{
		_states = new Dictionary<States, IState>
		{
			{ States.Bootstrap, new BootstrapState() },
			{ States.Title, new TitleState() },
			{ States.Recruitment, new RecruitmentState() },
			{ States.Gameplay, new GameplayState() },
			{ States.Victory, new VictoryState() },
		};

		_machine = new StateMachine<States, Triggers>(States.Bootstrap);
		_machine.OnTransitioned(OnTransitioned);

		_machine.Configure(States.Bootstrap)
			.Permit(Triggers.StartRecruitment, States.Recruitment);
		_machine.Configure(States.Recruitment)
			.Permit(Triggers.StartLevel, States.Gameplay);
		_machine.Configure(States.Gameplay)
			.Permit(Triggers.FinishLevel, States.Recruitment)
			.Permit(Triggers.Win, States.Victory)
			.Permit(Triggers.Lose, States.Bootstrap);
		_machine.Configure(States.Victory);
	}

	public void Start()
	{
		_currentState = _states[_machine.State];
		_currentState.Enter(null);
	}

	public void Tick()
	{
		_currentState?.Tick();
	}

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
			await _states[(States)destination.Superstate.UnderlyingState].Enter(transition.Parameters);
		}

		await _currentState.Enter(transition.Parameters);
	}
}

public interface IState
{
	Task Enter(object[] parameters);
	void Tick();
	Task Exit();
}
