using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.SecretSanta.Game.RPG
{
	public class PerformAttackState : BaseBattleState, IState
	{
		public PerformAttackState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public async Task Enter(object[] args)
		{
			_ui.InitActionMenu(_turn.Unit);

			await PerformAttack(_turn.Unit);
			_turn.HasActed = true;

			_machine.Fire(BattleStateMachine.Triggers.Done);
		}

		public async Task Exit()
		{
			_ui.InitActionMenu(null);
		}

		public void Tick() { }

		private async Task PerformAttack(Unit unit)
		{
			var units = _turnManager.GetActiveUnits();
			var destination = _turn.AttackPath[_turn.AttackPath.Count - 1];
			var targets = _turn.AttackTargets
				.Select(position => units.Find(unit => unit.GridPosition == position))
				.Where(unit => unit != null)
				.ToList();
			var result = new AttackResult
			{
				Attacker = _turn.Unit,
				Targets = targets,
				Path = _turn.AttackPath,
			};

			await unit.Facade.AnimateAttack(destination);

			await ShootProjectile(result.Attacker.GridPosition, destination);

			var hitTasks = new List<UniTask>();
			foreach (var target in result.Targets)
			{
				var hitChance = GridHelpers.GetHitAccuracy(_turn.Unit.GridPosition, target.GridPosition, _turnManager.BlockGrid, _turn.Unit);

				var didHit = Random.Range(0, 100) < hitChance;
				if (didHit)
				{
					hitTasks.Add(ApplyDamage(target, result.Attacker.HitDamage));
				}
				else
				{
					hitTasks.Add(Miss(target));
				}
			}

			await UniTask.WhenAll(hitTasks);
		}

		// TODO: Do this in Projectile.cs
		private async Task ShootProjectile(Vector3 origin, Vector3 destination)
		{
			var instance = GameObject.Instantiate(_config.SnowballPrefab, origin, Quaternion.identity);
			var distance = Vector3.Distance(origin, destination);
			await instance.transform.DOMove(destination, distance * 0.03f).SetEase(Ease.Linear);

			_spawner.SpawnEffect(_config.HitEffectPrefab, destination);

			// TODO: Use polling
			GameObject.Destroy(instance);
		}

		private async UniTask ApplyDamage(Unit unit, int amount)
		{
			Debug.Log($"{unit} hit for {amount} damage!");

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

			await UniTask.WhenAll(tasks);
		}

		private UniTask Miss(Unit unit)
		{
			Debug.Log($"{unit} evaded!");

			return _spawner.SpawnText(
				_config.DamageTextPrefab,
				"Miss",
				unit.GridPosition + Vector3.up
			);
		}
	}
}
