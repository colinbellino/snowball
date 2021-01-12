using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Snowball.Game
{
	public class PerformActionState : BaseBattleState
	{
		public PerformActionState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override async UniTask Enter()
		{
			await base.Enter();

			_ui.SetTurnUnit(_turn.Unit);

			switch (_turn.Action)
			{
				case Turn.Actions.Attack:
					await PerformAttack();
					break;
				case Turn.Actions.Build:
					await PerformBuild();
					break;
				case Turn.Actions.Melt:
					await PerformMelt();
					break;
				default:
					Debug.LogWarning("No action selected but we still tried to perform it ? We probably have a bug in our state machine.");
					break;
			}
			_turn.HasActed = true;

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}

		public override UniTask Exit()
		{
			base.Exit();

			_ui.SetTurnUnit(null);

			return default;
		}

		private async UniTask PerformAttack()
		{
			var units = _turnManager.GetActiveUnits();
			var targets = _turn.ActionTargets
				.Select(position => units.Find(unit => unit.GridPosition == position))
				.Where(unit => unit != null)
				.ToList();
			var result = new AttackResult
			{
				Attacker = _turn.Unit,
				Targets = targets,
			};

			var aimDirection = ((Vector3)(_turn.ActionDestination.Value - _turn.Unit.GridPosition)).normalized;
			var direction = UnitHelpers.VectorToDirection(aimDirection);
			var needsToChangeDirection = direction != _turn.Unit.Direction;

			if (needsToChangeDirection)
			{
				await _turn.Unit.Facade.AnimateChangeDirection(direction);
				_turn.Unit.Direction = direction;
			}
			await _turn.Unit.Facade.AnimateAttack(aimDirection);

			await ShootProjectile(result.Attacker.GridPosition, _turn.ActionDestination.Value);

			var hitTasks = new List<UniTask>();
			foreach (var target in result.Targets)
			{
				var hitChance = GridHelpers.CalculateHitAccuracy(
					_turn.Unit.GridPosition,
					target.GridPosition,
					_turnManager.BlockGrid,
					_turn.Unit,
					_turnManager.SortedUnits
				);

				var roll = Random.Range(0, 100);
				var didHit = roll < hitChance;
				if (didHit)
				{
					Debug.Log($"{_turn.Unit} hit for {result.Attacker.HitDamage} damage! ({roll}/{hitChance})");
					hitTasks.Add(ApplyDamage(target, result.Attacker.HitDamage));
				}
				else
				{
					Debug.Log($"{_turn.Unit} evaded! ({roll}/{hitChance})");
					hitTasks.Add(Miss(target));
				}
			}

			await UniTask.WhenAll(hitTasks);
		}

		private async UniTask PerformBuild()
		{
			var direction = UnitHelpers.VectorToDirection(_turn.ActionDestination.Value - _turn.Unit.GridPosition);
			var needsToChangeDirection = direction != _turn.Unit.Direction;

			if (needsToChangeDirection)
			{
				await _turn.Unit.Facade.AnimateChangeDirection(direction);
				_turn.Unit.Direction = direction;
			}
			await _turn.Unit.Facade.AnimateBuild(direction);

			var newUnit = new Unit(_database.Units[_config.SnowmanUnitId]);
			var facade = UnitHelpers.SpawnUnitFacade(
				_config.UnitPrefab,
				newUnit,
				_turn.ActionDestination.Value,
				Unit.Drivers.Computer,
				Unit.Alliances.Ally,
				direction
			);
			newUnit.SetFacade(facade);

			await newUnit.Facade.AnimateSpawn();

			_turnManager.SortedUnits.Add(newUnit);
		}

		private async UniTask PerformMelt()
		{
			_turn.Unit.HealthCurrent = Math.Max(_turn.Unit.HealthCurrent - 1, 0);

			await _turn.Unit.Facade.AnimateMelt();

			_turnManager.SortedUnits.Remove(_turn.Unit);
		}

		// TODO: Do this in Projectile.cs
		private async UniTask ShootProjectile(Vector3 origin, Vector3 destination)
		{
			var instance = GameObject.Instantiate(_config.SnowballPrefab, origin, Quaternion.identity);
			var distance = Vector3.Distance(origin, destination);
			await instance.transform.DOMove(destination, distance * 0.03f).SetEase(Ease.Linear);

			_spawner.SpawnEffect(_config.HitEffectPrefab, destination);

			// TODO: Use polling
			GameObject.Destroy(instance);
		}

		private UniTask ApplyDamage(Unit unit, int amount)
		{
			unit.HealthCurrent = Math.Max(unit.HealthCurrent - amount, 0);

			var tasks = new List<UniTask>();
			if (unit.HealthCurrent <= 0)
			{
				tasks.Add(unit.Facade.AnimateDeath());
			}

			var spawnDamageText = _spawner.SpawnText(
				_config.DamageTextPrefab,
				amount.ToString(),
				unit.GridPosition + Vector3.up
			);
			tasks.Add(spawnDamageText);

			return UniTask.WhenAll(tasks);
		}

		private UniTask Miss(Unit unit)
		{
			return _spawner.SpawnText(
				_config.DamageTextPrefab,
				"Miss",
				unit.GridPosition + Vector3.up
			);
		}
	}
}
