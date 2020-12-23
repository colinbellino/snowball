using UnityEngine;
using UnityEngine.Assertions;

namespace Code.SecretSanta.Game.RPG
{
	public class Game
	{
		public GameConfig Config { get; private set; }
		public EffectsManager Effects { get; private set; }
		public GameControls Controls { get; private set; }

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

		public Game()
		{
			Config = Resources.Load<GameConfig>("RPGConfig");
			Effects = new EffectsManager();
			Controls = new GameControls();
			Assert.IsNotNull(Config);
		}

		public class EffectsManager
		{
			public void Spawn(ParticleSystem effectPrefab, Vector3 position)
			{
				var instance = GameObject.Instantiate(effectPrefab, position, Quaternion.identity);
			}
		}
	}
}
