using DG.Tweening;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class CursorHover : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _spriteRenderer;

		private Sequence _sequence;

		private void OnEnable()
		{
			var originalPosition = _spriteRenderer.transform.localPosition;
			_sequence = DOTween.Sequence()
				.Append(_spriteRenderer.transform.DOLocalMoveY(originalPosition.y + 0.2f, 0.75f))
				.Append(_spriteRenderer.transform.DOLocalMoveY(originalPosition.y, 0.75f))
				.SetLoops(-1);
		}

		public void OnDisable()
		{
			_sequence.Kill();
		}
	}
}
