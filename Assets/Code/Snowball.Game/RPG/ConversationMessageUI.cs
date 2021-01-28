using CGTespy.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Snowball.Game
{
	public class ConversationMessageUI : MonoBehaviour
	{
		[SerializeField] private RectTransform _transform;
		[SerializeField] private VerticalLayoutGroup _layout;
		[SerializeField] private Text _name;
		[SerializeField] private Text _text;
		[SerializeField] public Image _bodyImage;
		[SerializeField] public Image _leftHandImage;
		[SerializeField] public Image _rightHandImage;

		public async UniTask SetMessage(ConversationMessage message, Color teamColor)
		{
			_name.text = $"<color=\"#{ColorUtility.ToHtmlStringRGB(message.Unit.ColorCloth)}\">{message.Unit.Name}</color>";
			_text.text = message.Text;
			_transform.ApplyAnchorPreset(message.Alignment, alsoSetPosition: true);

			_bodyImage.sprite = message.Unit.Sprite;
			_bodyImage.material = Instantiate(_bodyImage.material);
			UnitHelpers.ApplyColors(_bodyImage.material, teamColor, message.Unit.ColorHair, message.Unit.ColorSkin);
			UnitHelpers.ApplyColors(_leftHandImage.material, message.Unit.ColorCloth, message.Unit.ColorHair, message.Unit.ColorSkin);
			UnitHelpers.ApplyColors(_rightHandImage.material, message.Unit.ColorCloth, message.Unit.ColorHair, message.Unit.ColorSkin);

			_layout.enabled = false;
			await UniTask.NextFrame();
			_layout.enabled = true;
		}
	}
}
