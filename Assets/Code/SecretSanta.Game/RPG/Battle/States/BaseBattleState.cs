using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public abstract class BaseBattleState
	{
		private readonly Game _game;
		protected readonly BattleStateMachine _machine;
		protected readonly TurnManager _turnManager;

		protected GameConfig _config => _game.Config;
		protected GameControls _controls => _game.Controls;
		protected Camera _camera => _game.Camera;
		protected BattleUI _ui => _game.BattleUI;
		protected GameState _state => _game.State;
		protected Database _database => _game.Database;
		protected StuffSpawner _spawner => _game.Spawner;
		protected Board _board => _game.Board;
		protected Turn _turn => _turnManager.Turn;

		protected BaseBattleState(BattleStateMachine machine, TurnManager turnManager)
		{
			_machine = machine;

			_turnManager = turnManager;
			_game = Game.Instance;
		}
	}
}
