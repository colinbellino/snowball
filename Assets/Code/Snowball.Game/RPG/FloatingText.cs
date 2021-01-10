using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Snowball.Game
{
	public class FloatingText : MonoBehaviour
	{
		[SerializeField] private Text _text;
		[SerializeField] private Text _shadow;

		public UniTask Show(string text)
		{
			_text.text = text;
			_shadow.text = text;

			const float duration = 0.75f;

			_text.transform.DOMoveY(_text.transform.position.y + 0.5f, duration);
			_shadow.transform.DOMoveY(_shadow.transform.position.y + 0.5f, duration);

			return UniTask.Delay(TimeSpan.FromSeconds(duration));
		}
	}
}
