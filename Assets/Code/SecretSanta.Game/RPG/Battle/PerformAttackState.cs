using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class PerformAttackState : BaseBattleState, IState
	{
		public PerformAttackState(BattleStateMachine machine) : base(machine) { }

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
			var targets = _turn.AttackTargets
				.Select(position => _battle.Units.Find(unit => unit.GridPosition == position))
				.Where(unit => unit != null)
				.ToList();
			var result = new AttackResult
			{
				Attacker = _turn.Unit,
				Targets = targets,
				Path = _turn.AttackPath,
			};

			var destination = result.Path[result.Path.Count - 1];

			await unit.Facade.AnimateAttack(destination);

			await ShootProjectile(result.Attacker.GridPosition, destination);

			var damageTasks = new List<UniTask>();
			foreach (var target in result.Targets)
			{
				damageTasks.Add(ApplyDamage(target, 1));
			}

			await UniTask.WhenAll(damageTasks);
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
			Debug.Log($"{unit} hit for {amount} damage.");
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
	}
}
