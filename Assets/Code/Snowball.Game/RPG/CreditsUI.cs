using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Snowball.Game
{
	public class CreditsUI : MonoBehaviour
	{
		[SerializeField] private Transform _root;
		[SerializeField] private Text _text;

		private void Awake()
		{
			Hide();
		}

		public void Show()
		{
			_text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0f);
			_text.DOFade(1f, 0.5f);

			_root.gameObject.SetActive(true);
		}

		public void Hide()
		{
			_root.gameObject.SetActive(false);
		}
	}
}
