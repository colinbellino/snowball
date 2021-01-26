using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Snowball.Game
{
	public class UnitFacade : MonoBehaviour
	{
		[SerializeField] public Transform BodyPivot;
		[SerializeField] public SpriteRenderer BodyRenderer;
		[SerializeField] public SpriteRenderer LeftHandRenderer;
		[SerializeField] public SpriteRenderer RightHandRenderer;
		[SerializeField] public AudioSource AudioSource;

		[Title("Infos")]
		[SerializeField] public Canvas InfosCanvas;
		[SerializeField] public Text InfosNameText;
		[SerializeField] public Image InfosBodyImage;
		[SerializeField] public Image InfosLeftHandImage;
		[SerializeField] public Image InfosRightHandImage;
		[SerializeField] public Text InfosHealthText;
		[SerializeField] public Text InfosHitText;
		[SerializeField] public Image InfosHealthImage;
	}
}
