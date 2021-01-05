using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.SecretSanta.Game.RPG
{
	public class UnitFacade : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _bodyRenderer;
		[SerializeField] private Text _debugText;

		public Unit Unit { get; private set; }

		public override string ToString() => name;

		public void Initialize(Unit unit)
		{
			Unit = unit;
			SetName(Unit.Name);
			SetColor(Unit.Color);
		}

		public void SetPlayerControlled(bool value) => Unit.IsPlayerControlled = value;

		public void SetGridPosition(Vector3Int position)
		{
			Unit.GridPosition = position;
			transform.position = new Vector3(position.x, position.y, 0f);
			SetDebugText($"[{position.x},{position.y}]");
		}

		public void SetDebugText(string value)
		{
			_debugText.text = value;
		}

		public async Task MoveOnPath(List<Vector3Int> path)
		{
			for (var index = 1; index < path.Count; index++)
			{
				await Move(path[index]);
			}

			SetGridPosition(path[path.Count - 1]);
		}

		public async UniTask Turn(Vector3Int direction)
		{
			Unit.Direction = direction;

			await _bodyRenderer.transform.DORotate(new Vector3(0f, direction.x > 0 ? 0f : 180f, 0f), 0.15f);
		}

		public async Task Attack(AttackResult result)
		{
			var origin = _bodyRenderer.transform.position;
			var destination = result.Path[result.Path.Count - 1];
			var direction = (origin + destination).normalized;
			var animDestination = origin.x + direction.x * 0.75f;
			await _bodyRenderer.transform.DOMoveX(animDestination, 0.1f);
			await _bodyRenderer.transform.DOMoveX(origin.x, 0.1f);

			await ShootProjectile(result.Attacker.Facade.transform.position, destination);

			foreach (var target in result.Targets)
			{
				target.Facade.ApplyDamage(1);
			}
		}

		private void SetName(string value)
		{
			name = $"{value}";
		}

		private void SetColor(Color color)
		{
			_bodyRenderer.material.SetColor("ReplacementColor0", color);
		}

		private async Task ShootProjectile(Vector3 origin, Vector3 destination)
		{
			var instance = GameObject.Instantiate(Game.Instance.Config.SnowballPrefab, origin, Quaternion.identity);
			var distance = Vector3.Distance(origin, destination);
			await instance.transform.DOMove(destination, distance * 0.05f).SetEase(Ease.Linear);

			Game.Instance.Spawner.SpawnEffect(Game.Instance.Config.HitEffectPrefab, destination);

			Destroy(instance);
		}

		private void ApplyDamage(int amount)
		{
			Game.Instance.Spawner.SpawnText(Game.Instance.Config.DamageTextPrefab, amount.ToString(), Unit.GridPosition + Vector3Int.up);
			Debug.Log($"{name} hit for {amount} damage.");
		}

		private async Task Move(Vector3Int destination)
		{
			var distance = Vector3.Distance(transform.position, destination);
			await transform.DOMove(destination, distance * 0.15f).SetEase(Ease.Linear);
		}
	}
}
