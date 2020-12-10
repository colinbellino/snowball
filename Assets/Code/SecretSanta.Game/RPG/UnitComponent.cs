using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class UnitComponent : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _bodyRenderer;

		public Vector2Int GridPosition { get; private set; }
		public bool IsPlayerControlled { get; private set; }

		public void UpdateData(Unit data, bool isPlayerControlled)
		{
			SetName(data.Name);
			_bodyRenderer.color = data.Color;
			IsPlayerControlled = isPlayerControlled;
		}

		public void SetGridPosition(Vector2Int position)
		{
			GridPosition = position;
			transform.position = new Vector3(position.x, position.y, 0f);
		}

		private void SetName(string value)
		{
			name = $"{value} (Unit)";
		}

		public async Task Attack(UnitComponent target)
		{
			var origin = _bodyRenderer.transform.position;
			var direction = (target.transform.position - transform.position).normalized;
			var destination = origin.x + direction.x * 0.75f;
			await _bodyRenderer.transform.DOMoveX(destination, 0.1f);
			await _bodyRenderer.transform.DOMoveX(origin.x, 0.1f);
		}

		public async Task Move(Vector2Int destination)
		{
			var worldDestination = new Vector3(destination.x, destination.y, 0f);
			await transform.DOMove(worldDestination, 0.5f);
			SetGridPosition(destination);
		}
	}
}
