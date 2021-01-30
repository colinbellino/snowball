using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Snowball.Game
{
	public class TransitionManager : MonoBehaviour
	{
		[SerializeField] [Required] private Image _pane1;
		[SerializeField] [Required] private Image _pane2;
		[SerializeField] [Required] private Image _pane3;
		[SerializeField] [Required] private Text _text;

		public async UniTask StartTransition(Color color, bool showText = false)
		{
			_pane1.color = _pane2.color = _pane3.color = color;

			_text.gameObject.SetActive(showText);

			await DOTween.Sequence()
				.SetUpdate(true)
				.Append(_pane1.rectTransform.DOAnchorPosX(0, 0.5f))
				.Join(_pane2.rectTransform.DOAnchorPosX(0, 0.5f).SetDelay(0.1f))
				.Join(_pane3.rectTransform.DOAnchorPosX(0, 0.5f).SetDelay(0.1f))
			;
			await UniTask.Delay(TimeSpan.FromSeconds(0.2f), ignoreTimeScale: true);
		}

		public async UniTask EndTransition(bool showText = false)
		{
			_text.gameObject.SetActive(showText);

			await DOTween.Sequence()
				.SetUpdate(true)
				.Append(_pane1.rectTransform.DOAnchorPosX(-1920f, 0.5f))
				.Join(_pane2.rectTransform.DOAnchorPosX(-1920f, 0.5f).SetDelay(0.1f))
				.Join(_pane3.rectTransform.DOAnchorPosX(-1920f, 0.5f).SetDelay(0.1f))
			;
			await UniTask.Delay(TimeSpan.FromSeconds(0.2f), ignoreTimeScale: true);
		}

		public void SetText(string text, Color color)
		{
			_text.text = text;
			_text.color = color;
		}
	}
}
