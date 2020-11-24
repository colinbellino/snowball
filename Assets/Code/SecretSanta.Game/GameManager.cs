using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Entity entityPrefab;
	[SerializeField] private Spawn[] _spawns;

	private Queue<Spawn> _spawnsQueue;
	private GameState _gameState;

	public static Action<Entity> EntityDestroyed;
	public static readonly Vector2 AreaSize = new Vector2(32, 18);

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void Init()
	{
		EntityDestroyed = null;
	}

	void Awake()
	{
		_gameState = new GameState();
		_gameState.Inputs = new PlayerInput();

		_spawnsQueue = new Queue<Spawn>();
		foreach (var spawn in _spawns)
		{
			_spawnsQueue.Enqueue(spawn);
		}
	}

	void Start()
	{
		var player = GameObject.Instantiate(entityPrefab);
		player.Transform.position = new Vector3(0, -6);
		_gameState.Player = player;
	}

	void Update()
	{
		_gameState.Inputs.Move = new Vector2(
			Input.GetAxis("Horizontal"),
			Input.GetAxis("Vertical")
		);
		_gameState.Inputs.Fire = Input.GetButton("Jump");

		if (_gameState.Player)
		{
			_gameState.Player.Movement.MoveInDirection(_gameState.Inputs.Move, true);

			if (_gameState.Inputs.Fire)
			{
				_gameState.Player.Weapon.Fire();
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
	public Entity Player;
}

public class PlayerInput
{
	public Vector2 Move;
	public bool Fire;
}

[Serializable]
public class Spawn
{
	public float Time;
	public Vector2 Position;
	public Entity Prefab;
}
