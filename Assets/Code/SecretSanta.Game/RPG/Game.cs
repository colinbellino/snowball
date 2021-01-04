using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public class Game
	{
		public GameConfig Config { get; }
		public StuffSpawner Spawner { get; }
		public GameControls Controls { get; }
		public Tilemap AreaTilemap { get; }
		public Tilemap HighlightTilemap { get; }
		public GameObject WorldmapRoot { get; }
		public Battle Battle { get; }
		public Camera Camera { get; }
		public BattleUI BattleUI { get; }
		public DebugUI DebugUI { get; }

		private static Game _instance;
		public static Game Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Game();
				}
				return _instance;
			}
		}

		private Game()
		{
			Config = Resources.Load<GameConfig>("RPGConfig");
			AreaTilemap = GameObject.Find("Area").GetComponent<Tilemap>();
			HighlightTilemap = GameObject.Find("Highlight").GetComponent<Tilemap>();
			WorldmapRoot = GameObject.Find("Worldmap Grid");
			BattleUI = GameObject.FindObjectOfType<BattleUI>();
			DebugUI = GameObject.FindObjectOfType<DebugUI>();
			Camera = Camera.main;
			Spawner = new StuffSpawner();
			Controls = new GameControls();
			Battle = new Battle();

			Assert.IsNotNull(Config);
			Assert.IsNotNull(AreaTilemap);
			Assert.IsNotNull(HighlightTilemap);
			Assert.IsNotNull(DebugUI);
			Assert.IsNotNull(BattleUI);
			Assert.IsNotNull(Camera);
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void Init()
		{
			_instance = null;
		}
	}
}
