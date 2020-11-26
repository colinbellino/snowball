using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameConfig _config;

	public GameConfig Config => _config;
	public GameState State { get; } = new GameState();
	public GameUI GameUI { get; private set; }
	public GameStateMachine Machine { get; private set; }

	public static GameManager Instance { get; private set; }

	private GameControls _controls;

	private void Awake()
	{
		Instance = this;

		Machine = new GameStateMachine();
		GameUI = FindObjectOfType<GameUI>();
		_controls = new GameControls();
		_controls.Enable();
	}

	private void Start()
	{
		Machine.Start();
	}

	private void Update()
	{
		State.Inputs.Move = _controls.Gameplay.Move.ReadValue<Vector2>();
		State.Inputs.Fire = _controls.Gameplay.Fire.ReadValue<float>() > 0f;

		Machine.Tick();
	}
}

public class GameState
{
	public PlayerInput Inputs;
	public List<Recruit> Team;
	public Queue<Recruit> RecruitsQueue;
	public Queue<Spawn> SpawnsQueue;
}

public class PlayerInput
{
	public Vector2 Move;
	public bool Fire;
}
