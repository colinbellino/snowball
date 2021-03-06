﻿using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using static Snowball.Game.UnitHelpers;

namespace Snowball.Game
{
	public class AttackAbility : IAbility
	{
		private readonly GameConfig _config;
		private readonly StuffSpawner _spawner;

		public AttackAbility(GameConfig config, StuffSpawner spawner)
		{
			_config = config;
			_spawner = spawner;
		}

		public bool RequiresUnitTarget => true;
		public bool DoesDamage => true;

		public bool IsValidTarget(Unit actor, Unit target)
		{
			return target != null && target.HealthCurrent > 0 && target.Alliance != actor.Alliance && target.Type == Unit.Types.Humanoid;
		}

		public List<Vector3Int> GetAreaOfEffect(Vector3Int destination)
		{
			return new List<Vector3Int> { destination };
		}

		public List<Vector3Int> GetTilesInRange(Vector3Int origin, Unit actor, TurnManager turnManager)
		{
			return GridHelpers.GetTilesInRange(origin, actor.HitRange, turnManager.EmptyGrid);
		}

		public async UniTask Perform(TurnManager turnManager)
		{
			var actor = turnManager.Turn.Unit;
			var plan = turnManager.Turn.Plan;

			var activeUnits = turnManager.GetActiveUnits();
			var targets = new List<Unit>(); // TODO: Get This from ability area

			var destinationUnit = activeUnits.Find(unit => unit.GridPosition == plan.ActionDestination);
			if (destinationUnit != null)
			{
				targets.Add(destinationUnit);
			}

			var aimDirection = ((Vector3)(plan.ActionDestination - actor.GridPosition)).normalized;
			var direction = VectorToDirection(aimDirection);

			var needsToChangeDirection = direction != actor.Direction;
			if (needsToChangeDirection)
			{
				await AnimateChangeDirection(actor.Facade, direction);
				actor.Direction = direction;
			}

			await AnimateAttack(actor.Facade, aimDirection);

			await ShootProjectile(actor.GridPosition, plan.ActionDestination);

			var hitTasks = new List<UniTask>();
			foreach (var target in targets)
			{
				var hitChance = GridHelpers.CalculateHitAccuracy(
					actor.GridPosition,
					target.GridPosition,
					actor,
					turnManager.BlockGrid,
					turnManager.GetActiveUnits()
				);

				var roll = Random.Range(0, 100);
				var didHit = roll < hitChance;
				if (didHit)
				{
					Debug.Log($"{actor} hit for {actor.HitDamage} damage! ({roll}/{hitChance})");
					var damage = actor.HitDamage + Random.Range(0, actor.HitDamageVariation + 1);
					hitTasks.Add(Hit(target, damage));
				}
				else
				{
					Debug.Log($"{actor} missed! ({roll}/{hitChance})");
					hitTasks.Add(Evade(target));
				}
			}

			await UniTask.WhenAll(hitTasks);
		}

		// TODO: Move this to a ProjectileSpawner ? Something like _projectileSpawner.Spawn(origin, destination)
		private async UniTask ShootProjectile(Vector3 origin, Vector3 destination)
		{
			var instance = GameObject.Instantiate(_config.SnowballPrefab, origin, Quaternion.identity);
			var distance = Vector3.Distance(origin, destination);
			await instance.transform.DOMove(destination, distance * 0.03f).SetEase(Ease.Linear);

			_spawner.SpawnEffect(_config.HitEffectPrefab, destination);

			// TODO: Use polling
			GameObject.Destroy(instance);
		}

		private async UniTask Hit(Unit unit, int amount)
		{
			PlaySound(unit.Facade, _config.UnitHitClip);
			unit.HealthCurrent = Math.Max(unit.HealthCurrent - amount, 0);

			await UniTask.WhenAll(
				AnimateHit(unit.Facade, (int) unit.Direction),
				_spawner.SpawnText(_config.DamageTextPrefab, amount.ToString(), unit.GridPosition + Vector3.up)
			);

			if (unit.HealthCurrent <= 0)
			{
				PlaySound(unit.Facade, _config.UnitDeathClip);
				await AnimateDeath(unit.Facade);
			}
		}

		private async UniTask Evade(Unit unit)
		{
			PlaySound(unit.Facade, _config.UnitMissClip);

			await UniTask.WhenAll(
				AnimateEvade(unit.Facade, (int) unit.Direction),
				_spawner.SpawnText(_config.DamageTextPrefab, "Miss", unit.GridPosition + Vector3.up)
			);
		}
	}
}
