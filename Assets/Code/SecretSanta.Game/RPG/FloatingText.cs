using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.SecretSanta.Game.RPG
{
	public class FloatingText : MonoBehaviour
	{
		[SerializeField] private Text _text;
		[SerializeField] private Text _shadow;

		public float Duration => 0.75f;

		public async void Show(string text)
		{
			_text.text = text;
			_shadow.text = text;

			_text.transform.DOMoveY(_text.transform.position.y + 0.5f, Duration);
			_shadow.transform.DOMoveY(_shadow.transform.position.y + 0.5f, Duration);

			await UniTask.Delay(TimeSpan.FromSeconds(Duration));

			Destroy(gameObject);
		}
	}
}
