using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Spawn[] _spawns;
	[SerializeField] private Entity[] _recruits;
	private GameControls _controls;

	public GameState State  { get; private set; }
	public GameUI GameUI { get; private set; }
	public GameStateMachine Machine { get; private set; }

	public static GameManager Instance { get; private set; }

	public static readonly Vector2 AreaSize = new Vector2(32, 18);

	private void Awake()
	{
		Instance = this;

		Machine = new GameStateMachine();
		GameUI = FindObjectOfType<GameUI>();

		_controls = new GameControls();
		_controls.Enable();

		State = new GameState();
		State.Inputs = new PlayerInput();
		State.Team = new List<Entity>();

		State.SpawnsQueue = new Queue<Spawn>();
		foreach (var spawn in _spawns)
		{
			State.SpawnsQueue.Enqueue(spawn);
		}

		State.RecruitsQueue = new Queue<Entity>();
		var randomizedRecruits = _recruits
			.OrderBy(a => Guid.NewGuid())
			.ToList();
		foreach (var recruit in randomizedRecruits)
		{
			State.RecruitsQueue.Enqueue(recruit);
		}
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
	public List<Entity> Team;
	public Queue<Entity> RecruitsQueue;
	public Queue<Spawn> SpawnsQueue;
}

public class PlayerInput
{
	public Vector2 Move;
	public bool Fire;
}
