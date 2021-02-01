using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Snowball.Game
{
	public class CreditsUI : MonoBehaviour
	{
		[SerializeField] private Transform _root;
		[SerializeField] private Text _text;
		[SerializeField] private GameObject _menu;
		[SerializeField] private Button _newGameButton;
		[SerializeField] private Button _quitButton;

		public event Action NewGameClicked;
		public event Action QuitClicked;

		private void Awake()
		{
			Hide();
		}

		public void Show()
		{
			_root.gameObject.SetActive(true);

			_text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0f);
			_text.DOFade(1f, 0.5f);

			_menu.SetActive(true);

			_newGameButton.onClick.AddListener(OnNewGameClicked);
			_quitButton.onClick.AddListener(OnQuitClicked);
		}

		public void Hide()
		{
			_menu.SetActive(false);
			_root.gameObject.SetActive(false);
		}

		private void OnNewGameClicked()
		{
			NewGameClicked?.Invoke();
		}

		private void OnQuitClicked()
		{
			QuitClicked?.Invoke();
		}
	}
}
