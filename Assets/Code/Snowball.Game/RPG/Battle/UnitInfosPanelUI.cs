using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Snowball.Game
{
	public class UnitInfosPanelUI : MonoBehaviour
	{
		[Title("Infos")]
		[SerializeField] public GameObject InfosRoot;
		[SerializeField] public Text InfosNameText;
		[SerializeField] public Image InfosBodyImage;
		[SerializeField] public Image InfosLeftHandImage;
		[SerializeField] public Image InfosRightHandImage;
		[SerializeField] public Text InfosHealthText;
		[SerializeField] public Image InfosHealthImage;
	}
}
