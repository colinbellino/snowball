using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Snowball.Game
{
	public class StartBattleState : BaseBattleState
	{
		public StartBattleState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override async UniTask Enter()
		{
			await base.Enter();

			var encounter = _database.Encounters[_state.CurrentEncounter];

			Debug.Log($"Starting battle: {encounter.Name}");
			_board.DrawArea(encounter.Area);
			_board.ShowEncounter();

			var allUnits = new List<Unit>();
			for (var index = 0; index < encounter.Area.AllySpawnPoints.Count; index++)
			{
				var position = new Vector3Int(encounter.Area.AllySpawnPoints[index].x, encounter.Area.AllySpawnPoints[index].y, 0);
				var unit = new Unit(_database.Units[_state.Party[index]]);
				var facade = UnitHelpers.SpawnUnitFacade(
					_config.UnitPrefab,
					unit,
					position,
					Unit.Drivers.Human,
					Unit.Alliances.Ally,
					Unit.Directions.Right
				);
				unit.SetFacade(facade);

				allUnits.Add(unit);
			}

			for (var index = 0; index < encounter.Foes.Count; index++)
			{
				var position = new Vector3Int(encounter.Area.FoeSpawnPoints[index].x, encounter.Area.FoeSpawnPoints[index].y, 0);
				var unit = new Unit(encounter.Foes[index]);
				var facade = UnitHelpers.SpawnUnitFacade(
					_config.UnitPrefab,
					unit,
					position,
					Unit.Drivers.Computer,
					Unit.Alliances.Foe,
					Unit.Directions.Left
				);
				unit.SetFacade(facade);

				allUnits.Add(unit);
			}

			_controls.Enable();
			_turnManager.Start(allUnits, encounter.Area, _config.TilesData);

#if UNITY_EDITOR
			_board.DrawGridWalk(_turnManager.WalkGrid);
			_board.DrawBlockWalk(_turnManager.BlockGrid);
#endif

			await Game.Instance.Transition.EndTransition(Color.white);

			if (encounter.StartConversation.Length > 0)
			{
				await Game.Instance.Conversation.Start(encounter.StartConversation);
				var messages = new Queue<ConversationMessage>(new[]
				{
					new ConversationMessage { Unit = _database.Units[1], Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer felis mi, auctor in ex sed, dictum accumsan nisi." },
					new ConversationMessage { Unit = _database.Units[2], Text = "Quisque lacus justo, venenatis vitae semper sit amet, faucibus vel odio." },
					new ConversationMessage { Unit = _database.Units[1], Text = "Nunc dignissim nibh quam, in porta nulla imperdiet at." },
					new ConversationMessage { Unit = _database.Units[2], Text = "Interdum et malesuada fames ac ante ipsum primis in faucibus. Quisque elit tellus, lobortis in malesuada ut, egestas nec nisl. Vivamus porta placerat lacus, in porta tortor hendrerit vitae." },
				});
			}

			_machine.Fire(BattleStateMachine.Triggers.BattleStarted);
		}
	}
}
