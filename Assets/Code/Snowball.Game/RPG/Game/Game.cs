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
		public TitleUI TitleUI { get; }
		public CreditsUI CreditsUI { get; }
		public Board Board { get; }
		public Worldmap Worldmap { get; }
		public GameState State { get; }
		public Database Database { get; }
		public AudioPlayer AudioPlayer { get; }
		public TransitionManager Transition { get; }
		public ComputerPlayerUnit CPU { get; }

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
			TitleUI = GameObject.FindObjectOfType<TitleUI>();
			CreditsUI = GameObject.FindObjectOfType<CreditsUI>();
			Board = GameObject.FindObjectOfType<Board>();
			Worldmap = GameObject.FindObjectOfType<Worldmap>();
			Transition = GameObject.FindObjectOfType<TransitionManager>();
			Camera = Camera.main;
			Spawner = new StuffSpawner();
			Controls = new GameControls();
			State = new GameState();
			Database = new Database();
			CPU = new ComputerPlayerUnit();
			AudioPlayer = new AudioPlayer(Config, audioSource);

			if (suppressError == false)
			{
				Assert.IsNotNull(audioSource);
				Assert.IsNotNull(Config);
				Assert.IsNotNull(BattleUI);
				Assert.IsNotNull(TitleUI);
				Assert.IsNotNull(CreditsUI);
				Assert.IsNotNull(Board);
				Assert.IsNotNull(Worldmap);
				Assert.IsNotNull(Camera);
				Assert.IsNotNull(Transition);
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
	}
}
