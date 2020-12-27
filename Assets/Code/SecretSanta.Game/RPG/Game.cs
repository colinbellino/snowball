using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public class Game
	{
		public GameConfig Config { get; private set; }
		public EffectsManager Effects { get; private set; }
		public GameControls Controls { get; private set; }
		public Tilemap Tilemap { get; private set; }
		public Battle Battle { get; private set; }

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
			Effects = new EffectsManager();
			Controls = new GameControls();
			Tilemap = GameObject.FindObjectOfType<Tilemap>();
			Battle = new Battle();
			Assert.IsNotNull(Config);
		}

		public class EffectsManager
		{
			public void Spawn(ParticleSystem effectPrefab, Vector3 position)
			{
				GameObject.Instantiate(effectPrefab, position, Quaternion.identity);
			}
		}
	}
}
