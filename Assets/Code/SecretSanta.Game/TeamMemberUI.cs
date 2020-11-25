using UnityEngine;
using UnityEngine.UI;

public class TeamMemberUI : MonoBehaviour
{
	[SerializeField] private Text _nameText;
	[SerializeField] private Image _bodyRenderer;
	[SerializeField] private Image _coreRenderer;

	public void SetRecruit(Recruit recruit)
	{
		_nameText.text = recruit.Name;
		_coreRenderer.color = recruit.Color;
	}
}
