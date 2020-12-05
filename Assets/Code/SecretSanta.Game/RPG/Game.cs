using UnityEngine;
using UnityEngine.Assertions;

namespace Code.SecretSanta.Game.RPG
{
	public class Game
	{
		public GameConfig Config { get; private set; }

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
			Assert.IsNotNull(Config);
		}
	}
}
