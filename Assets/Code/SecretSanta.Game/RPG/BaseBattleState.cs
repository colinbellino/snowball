using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public abstract class BaseBattleState
	{
		protected readonly Battle _battle;
		protected readonly Tilemap _areaTilemap;
		protected readonly Tilemap _highlightTilemap;
		protected readonly GameConfig _config;
		protected readonly GameControls _controls;
		protected readonly BattleStateMachine _machine;
		protected readonly Camera _camera;
		protected readonly BattleUI _ui;
		protected readonly GameState _state;
		protected readonly Database _database;

		protected Turn _turn => _battle.Turn;

		protected BaseBattleState(BattleStateMachine machine)
		{
			_machine = machine;
			_battle = Game.Instance.Battle;
			_areaTilemap = Game.Instance.AreaTilemap;
			_highlightTilemap = Game.Instance.HighlightTilemap;
			_config = Game.Instance.Config;
			_controls = Game.Instance.Controls;
			_camera = Game.Instance.Camera;
			_ui = Game.Instance.BattleUI;
			_state = Game.Instance.State;
			_database = Game.Instance.Database;
		}
	}
}
