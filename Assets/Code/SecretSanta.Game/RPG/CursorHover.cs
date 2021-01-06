using DG.Tweening;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class CursorHover : MonoBehaviour
	{
		[SerializeField] private Transform _target;

		private Sequence _sequence;

		private void OnEnable()
		{
			var originalPosition = _target.localPosition;
			
			_sequence = DOTween.Sequence()
				.Append(_target.DOLocalMoveY(originalPosition.y + 0.2f, 0.75f))
				.Append(_target.DOLocalMoveY(originalPosition.y, 0.75f))
				.SetLoops(-1);
		}

		public void OnDisable()
		{
			_sequence.Kill();
		}
	}
}
