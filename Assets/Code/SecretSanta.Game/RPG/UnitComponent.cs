using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.SecretSanta.Game.RPG
{
	public class UnitComponent : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _bodyRenderer;
		[FormerlySerializedAs("_text")] [SerializeField] private Text _debugText;

		public Vector3Int GridPosition { get; private set; }
		public bool IsPlayerControlled { get; private set; }
		public Vector3Int Direction { get; private set; }

		public override string ToString() => name;

		public void UpdateData(Unit data, bool isPlayerControlled)
		{
			SetName(data.Name);
			_bodyRenderer.material.SetColor("ReplacementColor0", data.Color);

			IsPlayerControlled = isPlayerControlled;
		}

		public void SetGridPosition(Vector3Int position)
		{
			GridPosition = position;
			transform.position = new Vector3(position.x, position.y, 0f);
			SetDebugText($"[{position.x},{position.y}]");
		}

		public void SetDebugText(string value)
		{
			_debugText.text = value;
		}

		public async Task MoveOnPath(List<Vector3Int> path)
		{
			foreach (var move in path)
			{
				await Move(move);
			}
			SetGridPosition(path[path.Count - 1]);
		}

		public async UniTask Turn(Vector3Int direction)
		{
			await _bodyRenderer.transform.DORotate(new Vector3(0f, direction.x > 0 ? 0f : 180f, 0f), 0.15f);

			Direction = direction;
		}

		private void SetName(string value)
		{
			name = $"{value}";
		}

		public async Task Attack(AttackResult result)
		{
			var origin = _bodyRenderer.transform.position;
			var direction = (origin + result.Destination).normalized;
			var animDestination = origin.x + direction.x * 0.75f;
			await _bodyRenderer.transform.DOMoveX(animDestination, 0.1f);
			await _bodyRenderer.transform.DOMoveX(origin.x, 0.1f);

			await ShootProjectile(result.Attacker.transform.position, result.Destination);

			foreach (var target in result.Targets)
			{
				target.ApplyDamage(1);
			}
		}

		private async Task ShootProjectile(Vector3 origin, Vector3 destination)
		{
			var instance = GameObject.Instantiate(Game.Instance.Config.SnowballPrefab, origin, Quaternion.identity);
			var distance = Vector3.Distance(origin, destination);
			await instance.transform.DOMove(destination, distance * 0.05f).SetEase(Ease.Linear);

			Game.Instance.Effects.Spawn(Game.Instance.Config.HitEffectPrefab, destination);

			Destroy(instance);
		}

		private void ApplyDamage(int amount)
		{
			Debug.Log($"{name} hit for {amount} damage.");
		}

		private async Task Move(Vector3Int destination)
		{
			var distance = Vector3.Distance(transform.position, destination);
			await transform.DOMove(destination, distance * 0.15f).SetEase(Ease.Linear);
		}
	}
}
