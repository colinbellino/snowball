using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public abstract class BaseBattleState
	{
		protected readonly Battle _battle;
		protected readonly Tilemap _tilemap;
		protected readonly GameConfig _config;
		protected readonly GameControls _controls;
		protected readonly BattleStateMachine _machine;
		protected readonly Camera _camera;
		protected readonly BattleUI _ui;

		protected List<UnitComponent> _allUnits => _battle.Units;
		protected Turn _turn => _battle.Turn;

		protected BaseBattleState(BattleStateMachine machine)
		{
			_machine = machine;
			_battle = Game.Instance.Battle;
			_tilemap = Game.Instance.Tilemap;
			_config = Game.Instance.Config;
			_controls = Game.Instance.Controls;
			_camera = Game.Instance.Camera;
			_ui = Game.Instance.BattleUI;
		}
	}
}
