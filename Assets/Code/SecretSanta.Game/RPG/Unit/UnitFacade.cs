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

		public override string ToString() => name;

		public void Initialize(Unit unit)
		{
			SetName(unit.Name);
			SetColor(unit.Color);

			transform.position = new Vector3(unit.GridPosition.x, unit.GridPosition.y, 0f);
			_bodyRenderer.transform.Rotate(new Vector3(0f, unit.Direction.x > 0 ? 0f : 180f, 0f));
			SetDebugText($"[{unit.GridPosition.x},{unit.GridPosition.y}]");
		}

		public async Task MoveOnPath(List<Vector3Int> path)
		{
			for (var index = 1; index < path.Count; index++)
			{
				await MoveTo(path[index]);
			}
		}

		public async UniTask Turn(Vector3Int direction)
		{
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
			Game.Instance.Spawner.SpawnText(Game.Instance.Config.DamageTextPrefab, amount.ToString(), transform.position + Vector3.up);
			Debug.Log($"{name} hit for {amount} damage.");
		}

		private void SetName(string value)
		{
			name = $"{value}";
		}

		private void SetColor(Color color)
		{
			_bodyRenderer.material.SetColor("ReplacementColor0", color);
		}

		private void SetDebugText(string value)
		{
			_debugText.text = value;
		}

		private async Task MoveTo(Vector3Int destination)
		{
			var distance = Vector3.Distance(transform.position, destination);
			await transform.DOMove(destination, distance * 0.15f).SetEase(Ease.Linear);
		}
	}
}
