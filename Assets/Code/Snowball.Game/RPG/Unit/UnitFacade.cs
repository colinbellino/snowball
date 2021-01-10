using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Snowball.Game
{
	public class UnitFacade : MonoBehaviour
	{
		[SerializeField] private Transform _bodyPivot;
		[SerializeField] private SpriteRenderer _bodyRenderer;

		public override string ToString() => name;

		public void Initialize(Unit unit)
		{
			SetName(unit.Name);
			SetColor(unit.Color);

			transform.position = new Vector3(unit.GridPosition.x, unit.GridPosition.y, 0f);
			_bodyRenderer.sprite = unit.Sprite;
			_bodyRenderer.transform.Rotate(new Vector3(0f, unit.Direction > 0 ? 0f : 180f, 0f));
		}

		public async UniTask MoveOnPath(List<Vector3Int> path)
		{
			for (var index = 1; index < path.Count; index++)
			{
				await MoveTo(path[index]);
			}
		}

		public async UniTask AnimateChangeDirection(Unit.Directions direction)
		{
			await _bodyRenderer.transform.DORotate(new Vector3(0f, direction > 0 ? 0f : 180f, 0f), 0.15f);
		}

		public async UniTask AnimateAttack(Vector3 destination)
		{
			var origin = _bodyRenderer.transform.position;
			var direction = (origin + destination).normalized;
			var animDestination = origin.x + direction.x * 0.75f;
			await _bodyRenderer.transform.DOMoveX(animDestination, 0.1f);
			await _bodyRenderer.transform.DOMoveX(origin.x, 0.1f);
		}

		public async UniTask AnimateBuild(Unit.Directions direction)
		{
			var originalPosition = _bodyRenderer.transform.position;

			await DOTween.Sequence()
				.Append(_bodyRenderer.transform.DOMoveX(originalPosition.x + (direction == Unit.Directions.Right ? 0.2f : -0.2f), 0.2f))
				.Append(_bodyRenderer.transform.DOMoveX(originalPosition.x, 0.1f));
		}

		public async UniTask AnimateSpawn()
		{
			_bodyRenderer.transform.localScale = Vector3.zero;

			await _bodyRenderer.transform.DOScale(1, 0.3f);
		}

		public async UniTask AnimateDeath()
		{
			await _bodyRenderer.transform.DORotate(new Vector3(0f, 0f, 45f), 0.3f);
			await _bodyRenderer.transform.DOScale(0, 0.3f);
		}

		public async UniTask AnimateMelt()
		{
			await _bodyPivot.DOScaleY(0, 0.3f);
		}

		private void SetName(string value)
		{
			name = $"{value}";
		}

		private void SetColor(Color color)
		{
			_bodyRenderer.material.SetColor("ReplacementColor0", color);
		}

		private async UniTask MoveTo(Vector3Int destination)
		{
			const float durationPerUnit = 0.15f;

			var direction = destination.x - transform.position.x;

			if (direction != _bodyRenderer.transform.right.x && Mathf.Abs(direction) != 0)
			{
				await AnimateChangeDirection(direction > 0f ? Unit.Directions.Right : Unit.Directions.Left);
			}

			var distance = Vector3.Distance(transform.position, destination);
			await transform.DOMove(destination, distance * durationPerUnit).SetEase(Ease.Linear);
		}
	}
}
