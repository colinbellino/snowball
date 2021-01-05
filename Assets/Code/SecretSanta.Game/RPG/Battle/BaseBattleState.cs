using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public abstract class BaseBattleState
	{
		private readonly Game _game;
		protected readonly BattleStateMachine _machine;

		protected Battle _battle => _game.Battle;
		protected Tilemap _areaTilemap => _game.AreaTilemap;
		protected Tilemap _highlightTilemap => _game.HighlightTilemap;
		protected GameConfig _config => _game.Config;
		protected GameControls _controls => _game.Controls;
		protected Camera _camera => _game.Camera;
		protected BattleUI _ui => _game.BattleUI;
		protected GameState _state => _game.State;
		protected Database _database => _game.Database;
		protected StuffSpawner _spawner => _game.Spawner;
		protected Turn _turn => _battle.Turn;

		protected BaseBattleState(BattleStateMachine machine)
		{
			_machine = machine;
			_game = Game.Instance;
		}
	}
}
