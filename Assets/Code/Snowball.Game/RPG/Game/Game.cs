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
		public PauseUI PauseUI { get; }
		public Board Board { get; }
		public Worldmap Worldmap { get; }
		public GameState State { get; }
		public Database Database { get; }
		public AudioPlayer AudioPlayer { get; }
		public TransitionManager Transition { get; }
		public ComputerPlayerUnit CPU { get; }
		public Conversation Conversation { get; }
		public Pause Pause { get; }

		private ConversationUI ConversationUI;

		public static Game Instance { get; private set; }

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void InitializeOnLoad()
		{
			Instance = null;
		}

		private Game(bool suppressError = false)
		{
			var _musicAudioSource = GameObject.Find("Music Audio Source").GetComponent<AudioSource>();

			Config = Resources.Load<GameConfig>("Game Config");
			BattleUI = GameObject.FindObjectOfType<BattleUI>();
			TitleUI = GameObject.FindObjectOfType<TitleUI>();
			CreditsUI = GameObject.FindObjectOfType<CreditsUI>();
			ConversationUI = GameObject.FindObjectOfType<ConversationUI>();
			PauseUI = GameObject.FindObjectOfType<PauseUI>();
			Board = GameObject.FindObjectOfType<Board>();
			Worldmap = GameObject.FindObjectOfType<Worldmap>();
			Transition = GameObject.FindObjectOfType<TransitionManager>();
			Camera = Camera.main;
			Spawner = new StuffSpawner();
			Controls = new GameControls();
			State = new GameState();
			Database = new Database();
			Conversation = new Conversation(ConversationUI);
			Pause = new Pause(PauseUI);
			CPU = new ComputerPlayerUnit(Database, Config, Spawner);
			AudioPlayer = new AudioPlayer(Config, _musicAudioSource);

			if (suppressError == false)
			{
				Assert.IsNotNull(_musicAudioSource);
				Assert.IsNotNull(Config);
				Assert.IsNotNull(BattleUI);
				Assert.IsNotNull(TitleUI);
				Assert.IsNotNull(CreditsUI);
				Assert.IsNotNull(ConversationUI);
				Assert.IsNotNull(PauseUI);
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
	}
}
