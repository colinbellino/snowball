using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Snowball.Game
{
	public class ConversationUI : MonoBehaviour
	{
		[SerializeField] private GameObject _root;
		[SerializeField] private VerticalLayoutGroup _container;
		[SerializeField] private Text _text;
		[SerializeField] private Text _name;

		private void Awake()
		{
			Hide();
		}

		public void Show() => _root.SetActive(true);

		public void Hide() => _root.SetActive(false);

		public async UniTask SetMessage(ConversationMessage message)
		{
			_name.text = $"<color=\"#{ColorUtility.ToHtmlStringRGB(message.Unit.ColorCloth)}\">{message.Unit.Name}</color>";
			_text.text = message.Text;

			_container.enabled = false;
			await UniTask.NextFrame();
			_container.enabled = true;
		}
	}
}
