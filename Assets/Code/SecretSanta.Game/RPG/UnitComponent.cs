using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class UnitComponent : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _bodyRenderer;

		public Vector3Int GridPosition { get; private set; }
		public bool IsPlayerControlled { get; private set; }

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
		}

		private void SetName(string value)
		{
			name = $"{value}";
		}

		public async Task Attack(AttackResult result)
		{
			Debug.Log($"Attack ({(result.Direction > 0 ? "Right" : "Left")}): {result.Destination} -> {result.Target}");

			{
				var origin = _bodyRenderer.transform.position;
				var destination = origin.x + result.Direction * 0.75f;
				await _bodyRenderer.transform.DOMoveX(destination, 0.1f);
				await _bodyRenderer.transform.DOMoveX(origin.x, 0.1f);
			}

			// TODO: animate snowball

			if (result.Target)
			{
				Game.Instance.Effects.Spawn(Game.Instance.Config.HitEffect, result.Target.GridPosition);
				result.Target.ApplyDamage(1);
			}
		}

		private void ApplyDamage(int amount)
		{
			Debug.Log($"{name} hit for {amount} damage.");
		}

		private async Task Move(Vector3Int destination)
		{
			await transform.DOMove(destination, 0.3f).SetEase(Ease.Linear);
		}

		public async Task MoveOnPath(List<Vector3Int> path)
		{
			foreach (var move in path)
			{
				await Move(move);
			}
			SetGridPosition(path[path.Count - 1]);
		}
	}
}
