using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Spawn[] _spawns;
	[SerializeField] private Recruit[] _recruits;
	[SerializeField] private Entity _enemyPrefab;
	[SerializeField] private Entity _recruitPrefab;
	private GameControls _controls;

	public Entity EnemyPrefab => _enemyPrefab;
	public Entity RecruitPrefab => _recruitPrefab;
	public GameState State { get; private set; }
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
		State.Team = new List<Recruit>();

		State.SpawnsQueue = new Queue<Spawn>();
		foreach (var spawn in _spawns)
		{
			State.SpawnsQueue.Enqueue(spawn);
		}

		State.RecruitsQueue = new Queue<Recruit>();
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
	public List<Recruit> Team;
	public Queue<Recruit> RecruitsQueue;
	public Queue<Spawn> SpawnsQueue;
}

public class PlayerInput
{
	public Vector2 Move;
	public bool Fire;
}
