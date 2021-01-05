using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public class StartBattleState : BaseBattleState, IState
	{
		public StartBattleState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args)
		{
			var encounter = _database.Encounters[_state.CurrentEncounterId];

			Helpers.RenderArea(encounter.Area, _areaTilemap, _config.TilesData);

			var allUnits = new List<Unit>();
			for (var index = 0; index < _state.Party.Count; index++)
			{
				var position = new Vector3Int(encounter.Area.AllySpawnPoints[index].x, encounter.Area.AllySpawnPoints[index].y, 0);
				var unit = _state.Party[index];
				unit.SetFacade(SpawnFacade(unit, position, true));

				allUnits.Add(unit);
			}
			for (var index = 0; index < encounter.Foes.Count; index++)
			{
				var position = new Vector3Int(encounter.Area.FoeSpawnPoints[index].x, encounter.Area.FoeSpawnPoints[index].y, 0);
				var unit = new Unit(encounter.Foes[index]);
				unit.SetFacade(SpawnFacade(unit, position, false));

				allUnits.Add(unit);
			}

			Notification.Send("BattleStarted");
			_battle.Start(allUnits, encounter.Area, _config.TilesData);
			_controls.Enable();

			#if UNITY_EDITOR
			var gridWalkTilemap = GameObject.Find("GridWalk").GetComponent<Tilemap>();
			Helpers.RenderGridWalk(_battle.WalkGrid, gridWalkTilemap, _config.EmptyTile);
			#endif

			_machine.Fire(BattleStateMachine.Triggers.BattleStarted);
		}

		public async Task Exit() { }

		public void Tick() { }

		private UnitFacade SpawnFacade(Unit unit, Vector3Int position, bool isPlayerControlled)
		{
			var facade = GameObject.Instantiate(_config.UnitPrefab);
			unit.GridPosition = position;
			unit.Direction = isPlayerControlled ? Vector3Int.right : Vector3Int.left;
			unit.IsPlayerControlled = isPlayerControlled;
			facade.Initialize(unit);

			return facade;
		}
	}
}
