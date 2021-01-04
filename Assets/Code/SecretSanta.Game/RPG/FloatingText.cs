using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.SecretSanta.Game.RPG
{
	public class FloatingText : MonoBehaviour
	{
		[SerializeField] private Text _text;

		private Sequence _sequence;

		private void OnEnable()
		{
			_text.gameObject.SetActive(false);
		}

		public void OnDisable()
		{
			_sequence.Kill();
		}

		public void Show(string text)
		{
			_text.text = text;
			_text.gameObject.SetActive(true);

			_sequence = DOTween.Sequence()
				.Append(_text.transform.DOMoveY(_text.transform.position.y + 0.5f, 0.75f))
				.Append(_text.DOColor(Color.clear, 0.75f))
				.OnComplete(() => {
					// gameObject.SetActive(false);
					Destroy(gameObject);
				});
		}
	}
}
