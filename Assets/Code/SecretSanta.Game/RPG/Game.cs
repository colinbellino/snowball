using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public class Game
	{
		public GameConfig Config { get; }
		public EffectsManager Effects { get; }
		public GameControls Controls { get; }
		public Tilemap Tilemap { get; }
		public Battle Battle { get; }

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
