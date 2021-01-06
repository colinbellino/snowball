using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Code.SecretSanta.Game.RPG
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
		public GameState State { get; }
		public Database Database { get; }

		public static Game Instance { get; private set; }

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void InitializeOnLoad()
		{
			Instance = null;
		}

		private Game()
		{
			Config = Resources.Load<GameConfig>("RPGConfig");
			BattleUI = GameObject.FindObjectOfType<BattleUI>();
			DebugUI = GameObject.FindObjectOfType<DebugUI>();
			Board = GameObject.FindObjectOfType<Board>();
			Camera = Camera.main;
			Spawner = new StuffSpawner();
			Controls = new GameControls();
			State = new GameState();
			Database = new Database();

			Assert.IsNotNull(Config);
			Assert.IsNotNull(BattleUI);
			Assert.IsNotNull(DebugUI);
			Assert.IsNotNull(Board);
			Assert.IsNotNull(Camera);
		}

		public static void Init()
		{
			Instance = new Game();
		}

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

	// This is what will be persisted when saving the game
	public class GameState
	{
		public int CurrentEncounterId = 1;
		public List<Unit> Party;
	}
}
