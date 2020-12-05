using UnityEngine;
using UnityEngine.UI;

public class RecruitPanelUI : MonoBehaviour
{
	[SerializeField] private Text _idText;
	[SerializeField] private Image _bodyRenderer;
	[SerializeField] private Image _coreRenderer;
	[SerializeField] private Button _button;

	public Button Button => _button;

	public void SetRecruit(Recruit recruit)
	{
		_idText.text = recruit.Id;
		_coreRenderer.color = recruit.Color;
	}
}
