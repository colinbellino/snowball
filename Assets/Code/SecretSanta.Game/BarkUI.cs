using UnityEngine;
using UnityEngine.UI;

public class BarkUI : MonoBehaviour
{
	[SerializeField] private Transform _root;
	[SerializeField] private Text _text;
	[SerializeField] private Image _bodyRenderer;
	[SerializeField] private Image _coreRenderer;

	public void Show(Recruit recruit, Bark bark, float duration)
	{
		_root.gameObject.SetActive(true);
		_text.text = bark.Text;
		_coreRenderer.color = recruit.Color;

		Invoke(nameof(Hide), duration);
	}

	public void Hide()
	{
		_root.gameObject.SetActive(false);
	}
}
