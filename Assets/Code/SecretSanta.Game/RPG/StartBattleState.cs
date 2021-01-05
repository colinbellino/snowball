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

			var allUnits = new List<UnitRuntime>();
			for (var index = 0; index < _state.Party.Count; index++)
			{
				var position = new Vector3Int(encounter.Area.AllySpawnPoints[index].x, encounter.Area.AllySpawnPoints[index].y, 0);
				var unit = _state.Party[index];
				var facade = SpawnUnit(unit, position, true);
				unit.Facade = facade;

				allUnits.Add(unit);
			}
			for (var index = 0; index < encounter.Foes.Count; index++)
			{
				var position = new Vector3Int(encounter.Area.FoeSpawnPoints[index].x, encounter.Area.FoeSpawnPoints[index].y, 0);
				var unit = new UnitRuntime(encounter.Foes[index]);
				var facade = SpawnUnit(unit, position);
				unit.Facade = facade;

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

		private UnitComponent SpawnUnit(UnitRuntime data, Vector3Int position, bool isPlayerControlled = false)
		{
			var facade = GameObject.Instantiate(_config.UnitPrefab);
			facade.Initialize(data);
			facade.SetGridPosition(position);
			facade.Turn(isPlayerControlled ? Vector3Int.right : Vector3Int.left);
			facade.SetPlayerControlled(isPlayerControlled);
			return facade;
		}
	}
}
