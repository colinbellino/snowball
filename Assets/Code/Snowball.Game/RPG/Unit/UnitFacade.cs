using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Snowball.Game
{
	public class UnitFacade : MonoBehaviour
	{
		[SerializeField] private Transform _bodyPivot;
		[SerializeField] private SpriteRenderer _bodyRenderer;
		[SerializeField] private SpriteRenderer _leftHandRenderer;
		[SerializeField] private SpriteRenderer _rightHandRenderer;

		public override string ToString() => name;

		public void Initialize(Unit unit)
		{
			SetName(unit.Name);
			UnitHelpers.ApplyColors(_bodyRenderer.material, unit.Color, unit.Color2);

			transform.position = new Vector3(unit.GridPosition.x, unit.GridPosition.y, 0f);
			_bodyRenderer.sprite = unit.Sprite;
			_bodyRenderer.transform.Rotate(new Vector3(0f, unit.Direction > 0 ? 0f : 180f, 0f));
		}

		public async UniTask MoveOnPath(List<Vector3Int> path)
		{
			for (var index = 1; index < path.Count; index++)
			{
				await MoveTo(path[index], index % 2 == 0);
			}
		}

		public async UniTask AnimateChangeDirection(Unit.Directions direction)
		{
			await _bodyPivot.transform.DORotate(new Vector3(0f, direction > 0 ? 0f : 180f, 0f), 0.15f);
		}

		public async UniTask AnimateAttack(Vector3 aimDirection)
		{
			var origin = _rightHandRenderer.transform.position;

			await DOTween.Sequence()
				.Append(_leftHandRenderer.transform.DOMove(origin + aimDirection * 0.2f, 0.1f))
				.Join(_rightHandRenderer.transform.DOMove(origin - aimDirection * 0.2f, 0.1f))
				.Append(_rightHandRenderer.transform.DOMove(origin + aimDirection * 0.5f, 0.1f))
			;

			_leftHandRenderer.transform.DOMove(origin, 0.1f);
			_rightHandRenderer.transform.DOMove(origin, 0.1f);
		}

		public async UniTask AnimateBuild(Unit.Directions direction)
		{
			var originalPosition = _bodyRenderer.transform.position;

			await DOTween.Sequence()
				.Append(_leftHandRenderer.transform.DOLocalMove(_leftHandRenderer.transform.localPosition + new Vector3(0.1f, -0.2f, 0f), 0.1f))
				.Join(_rightHandRenderer.transform.DOLocalMove(_rightHandRenderer.transform.localPosition + new Vector3(0.1f, -0.2f, 0f), 0.1f))
				.Join(_bodyRenderer.transform.DOMoveX(originalPosition.x + (direction == Unit.Directions.Right ? 0.2f : -0.2f), 0.2f))
				.Append(_bodyRenderer.transform.DOMoveX(originalPosition.x, 0.1f))
				.Join(_leftHandRenderer.transform.DOLocalMove(_leftHandRenderer.transform.localPosition, 0.1f))
				.Join(_rightHandRenderer.transform.DOLocalMove(_rightHandRenderer.transform.localPosition, 0.1f));
		}

		public async UniTask AnimateSpawn()
		{
			_bodyRenderer.transform.localScale = Vector3.zero;

			await _bodyRenderer.transform.DOScale(1, 0.3f);
		}

		public async UniTask AnimateDeath()
		{
			await DOTween.Sequence()
				.Append(_leftHandRenderer.transform.DOLocalMove(_leftHandRenderer.transform.localPosition + new Vector3(0.2f, 0.4f, 0f), 0.1f))
				.Join(_rightHandRenderer.transform.DOLocalMove(_rightHandRenderer.transform.localPosition + new Vector3(-0.2f, 0.4f, 0f), 0.1f))
				.Append(_bodyRenderer.transform.DORotate(new Vector3(0f, 0f, 45f), 0.3f))
				.Append(_bodyRenderer.transform.DOScale(0, 0.3f))
			;
		}

		public async UniTask AnimateMelt()
		{
			await _bodyPivot.DOScaleY(0, 0.3f);
		}

		private void SetName(string value)
		{
			name = $"{value}";
		}

		private async UniTask MoveTo(Vector3Int destination, bool reverseHands = true)
		{
			const float durationPerUnit = 0.15f;

			var direction = destination.x - transform.position.x;

			if (direction != _bodyRenderer.transform.right.x && Mathf.Abs(direction) != 0)
			{
				await AnimateChangeDirection(direction > 0f ? Unit.Directions.Right : Unit.Directions.Left);
			}

			var distance = Vector3.Distance(transform.position, destination);
			var animDuration = distance * durationPerUnit;
			var handsOffset = reverseHands ? 1f : -1f;

			await DOTween.Sequence()
				.Append(transform.DOMove(destination, animDuration).SetEase(Ease.Linear))
				.Join(
					DOTween.Sequence()
						.Append(_leftHandRenderer.transform.DOLocalMove(_leftHandRenderer.transform.localPosition + new Vector3(+0.1f, +0.1f, 0f) * handsOffset, animDuration / 2))
						.Join(_rightHandRenderer.transform.DOLocalMove(_rightHandRenderer.transform.localPosition + new Vector3(-0.1f, -0.1f, 0f) * handsOffset, animDuration / 2))
						.Append(_leftHandRenderer.transform.DOLocalMove(_leftHandRenderer.transform.localPosition, animDuration / 2))
						.Join(_rightHandRenderer.transform.DOLocalMove(_rightHandRenderer.transform.localPosition, animDuration / 2))
				)
				.Join(DOTween.Sequence()
					.Append(_bodyPivot.transform.DOScale(new Vector3(0.9f, 1.1f, 1f), animDuration / 2))
					.Append(_bodyPivot.transform.DOScale(new Vector3(1f, 1f, 1f), animDuration / 2))
				)
			;
		}
	}
}
