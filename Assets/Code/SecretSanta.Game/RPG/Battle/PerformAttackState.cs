using System;
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

			foreach (var target in result.Targets)
			{
				ApplyDamage(target, 1);
			}
		}

		// TODO: Do this in Projectile.cs
		private async Task ShootProjectile(Vector3 origin, Vector3 destination)
		{
			var instance = GameObject.Instantiate(_config.SnowballPrefab, origin, Quaternion.identity);
			var distance = Vector3.Distance(origin, destination);
			await instance.transform.DOMove(destination, distance * 0.05f).SetEase(Ease.Linear);

			_spawner.SpawnEffect(_config.HitEffectPrefab, destination);

			// TODO: Use polling
			GameObject.Destroy(instance);
		}

		private void ApplyDamage(Unit unit, int amount)
		{
			unit.HealthCurrent = Math.Max(unit.HealthCurrent - amount, 0);
			_spawner.SpawnText(_config.DamageTextPrefab, amount.ToString(), unit.GridPosition + Vector3.up);
			Debug.Log($"{unit.Name} hit for {amount} damage.");
		}
	}
}
