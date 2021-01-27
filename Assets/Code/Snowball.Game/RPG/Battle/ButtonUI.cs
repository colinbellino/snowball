using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Snowball.Game
{
	public class ButtonUI : MonoBehaviour
	{
		[SerializeField][Required] private Button _button;
		[SerializeField][Required] private Text _text;
		[SerializeField][Required] private Color _defaultColor;
		[SerializeField][Required] private Color _inactiveColor;

		public Button.ButtonClickedEvent OnClick;

		private void Awake()
		{
			_button.onClick = OnClick;
		}

		public void Toggle(bool value)
		{
			_button.interactable = value;
			_text.color = value ? _defaultColor : _inactiveColor;
		}
	}
}
