using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameConfig _config;

	public GameConfig Config => _config;
	public GameState State { get; private set; }
	public GameStateMachine Machine { get; private set; }
	public BarkManager BarkManager { get; private set; }
	public GameUI GameUI { get; private set; }

	public static GameManager Instance { get; private set; }

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void Init()
	{
		Instance = null;
	}

	private void Awake()
	{
		State = new GameState();
		Machine = new GameStateMachine();
		BarkManager = new BarkManager();
		GameUI = FindObjectOfType<GameUI>();

		Instance = this;
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
	public int CurrentLevel;
	public GameControls Controls;
	public List<Recruit> Team;
	public Queue<Recruit> RecruitsQueue;
	public Queue<Spawn> SpawnsQueue;
}
