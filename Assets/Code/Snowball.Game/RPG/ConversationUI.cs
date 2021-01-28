using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Snowball.Game
{
	public class ConversationUI : MonoBehaviour
	{
		[SerializeField] private GameObject _root;
		[SerializeField] private ConversationMessageUI _messageUI;

		private void Awake()
		{
			Hide();
		}

		public void Show() => _root.SetActive(true);

		public void Hide() => _root.SetActive(false);

		public UniTask SetMessage(ConversationMessage message, Color teamColor) => _messageUI.SetMessage(message, teamColor);
	}
}
