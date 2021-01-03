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
			var encounter = _config.Encounters[0];
			Helpers.RenderArea(encounter.Area, _areaTilemap, _config.TilesData);

			var allUnits = new List<UnitComponent>();
			for (var index = 0; index < encounter.Allies.Count; index++)
			{
				var point = encounter.Area.AllySpawnPoints[index];
				allUnits.Add(SpawnUnit(encounter.Allies[index], new Vector3Int(point.x, point.y, 0), true));
			}
			for (var index = 0; index < encounter.Foes.Count; index++)
			{
				var point = encounter.Area.FoeSpawnPoints[index];
				allUnits.Add(SpawnUnit(encounter.Foes[index], new Vector3Int(point.x, point.y, 0)));
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

		private UnitComponent SpawnUnit(Unit data, Vector3Int position, bool isPlayerControlled = false)
		{
			var unit = GameObject.Instantiate(_config.UnitPrefab);
			unit.SetGridPosition(position);
			_ = unit.Turn(isPlayerControlled ? Vector3Int.right : Vector3Int.left);
			unit.UpdateData(data, isPlayerControlled);
			return unit;
		}
	}
}
