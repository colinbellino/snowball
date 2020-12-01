using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BarkUI : MonoBehaviour
{
	[SerializeField] private Transform _root;
	[SerializeField] private Text _text;
	[SerializeField] private Image _bodyRenderer;
	[SerializeField] private Image _coreRenderer;

	public async Task Show(Recruit recruit, Bark bark, float duration)
	{
		_root.gameObject.SetActive(true);
		_text.text = bark.Text;
		_coreRenderer.color = recruit.Color;

		await Task.Delay(TimeSpan.FromMilliseconds(duration));

		Hide();
	}

	public void Hide()
	{
		_root.gameObject.SetActive(false);
	}
}
