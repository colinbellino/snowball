using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Snowball.Game
{
	public class Game
	{
		public GameConfig Config { get; }
		public StuffSpawner Spawner { get; }
		public GameControls Controls { get; }
		public Camera Camera { get; }
		public BattleUI BattleUI { get; }
		public DebugUI DebugUI { get; }
		public Board Board { get; }
		public Worldmap Worldmap { get; }
		public GameState State { get; }
		public Database Database { get; }
		public AudioPlayer AudioPlayer { get; }

		public static Game Instance { get; private set; }

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void InitializeOnLoad()
		{
			Instance = null;
		}

		private Game(bool suppressError = false)
		{
			var audioSource = GameObject.FindObjectOfType<AudioSource>();
			Config = Resources.Load<GameConfig>("RPGConfig");
			BattleUI = GameObject.FindObjectOfType<BattleUI>();
			DebugUI = GameObject.FindObjectOfType<DebugUI>();
			Board = GameObject.FindObjectOfType<Board>();
			Worldmap = GameObject.FindObjectOfType<Worldmap>();
			Camera = Camera.main;
			Spawner = new StuffSpawner();
			Controls = new GameControls();
			State = new GameState();
			Database = new Database();
			AudioPlayer = new AudioPlayer(Config, audioSource);

			if (suppressError == false)
			{
				Assert.IsNotNull(audioSource);
				Assert.IsNotNull(Config);
				Assert.IsNotNull(BattleUI);
				Assert.IsNotNull(DebugUI);
				Assert.IsNotNull(Board);
				Assert.IsNotNull(Worldmap);
				Assert.IsNotNull(Camera);
			}
		}

		public static void Init()
		{
			Instance = new Game();
		}

		#if UNITY_EDITOR
		public static void InitForDebug()
		{
			Instance = new Game(true);
		}
		#endif

		public void LoadStateFromSave()
		{
			var party = new List<Unit>();
			foreach (var unitId in Config.StartingParty)
			{
				party.Add(new Unit(Database.Units[unitId]));
			}
			State.Party = party;
		}
	}
}
