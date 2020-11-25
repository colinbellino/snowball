using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	// [SerializeField] private Entity entityPrefab;
	[SerializeField] private Spawn[] _spawns;
	[SerializeField] private Entity[] _recruits;

	private Queue<Entity> _recruitsQueue;
	private Queue<Spawn> _spawnsQueue;
	private GameState _gameState;
	private GameControls _gameControls;

	public static Action<Entity> EntityDestroyed;
	public static readonly Vector2 AreaSize = new Vector2(32, 18);

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void Init()
	{
		EntityDestroyed = null;
	}

	private void Awake()
	{
		_gameControls = new GameControls();

		_gameState = new GameState();
		_gameState.Inputs = new PlayerInput();
		_gameState.Team = new List<Entity>();

		_spawnsQueue = new Queue<Spawn>();
		foreach (var spawn in _spawns)
		{
			_spawnsQueue.Enqueue(spawn);
		}

		_recruitsQueue = new Queue<Entity>();
		var randomizedRecruits = _recruits
			.OrderBy(a => Guid.NewGuid())
			.ToList();
		foreach (var recruit in randomizedRecruits)
		{
			_recruitsQueue.Enqueue(recruit);
		}
	}

	private void Start()
	{
		_gameControls.Enable();

		var player = GameObject.Instantiate(_recruitsQueue.Dequeue());
		player.Transform.position = new Vector3(0, -6);
		_gameState.Team.Add(player);
	}

	private void Update()
	{
		_gameState.Inputs.Move = _gameControls.Gameplay.Move.ReadValue<Vector2>();
		_gameState.Inputs.Fire = _gameControls.Gameplay.Fire.ReadValue<float>() > 0f;

		if (_gameState.Team.Count > 0)
		{
			_gameState.Team[0].Movement.MoveInDirection(_gameState.Inputs.Move, true);

			if (_gameState.Inputs.Fire)
			{
				_gameState.Team[0].Weapon.Fire();
			}
		}

		{
			if (_spawnsQueue.Count > 0)
			{
				var nextSpawn = _spawnsQueue.Peek();
				if (Time.time >= nextSpawn.Time)
				{
					var enemy = GameObject.Instantiate(nextSpawn.Prefab);
					enemy.Transform.position = nextSpawn.Position;
					_spawnsQueue.Dequeue();
				}
			}
		}
	}
}

public class GameState
{
	public PlayerInput Inputs;
	public List<Entity> Team;
}

public class PlayerInput
{
	public Vector2 Move;
	public bool Fire;
}
