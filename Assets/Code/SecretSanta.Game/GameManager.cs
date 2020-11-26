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

	private void Awake()
	{
		Instance = this;

		Machine = new GameStateMachine();
		GameUI = FindObjectOfType<GameUI>();
	}

	private void Start()
	{
		Machine.Start();
	}

	private void Update()
	{
		Machine.Tick();
	}
}

public class GameState
{
	public GameControls Controls;
	public List<Recruit> Team;
	public Queue<Recruit> RecruitsQueue;
	public Queue<Spawn> SpawnsQueue;
}
