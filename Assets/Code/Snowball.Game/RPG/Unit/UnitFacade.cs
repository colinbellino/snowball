using UnityEngine;

namespace Snowball.Game
{
	public class UnitFacade : MonoBehaviour
	{
		[SerializeField] public Transform BodyPivot;
		[SerializeField] public SpriteRenderer BodyRenderer;
		[SerializeField] public SpriteRenderer LeftHandRenderer;
		[SerializeField] public SpriteRenderer RightHandRenderer;
		[SerializeField] public AudioSource AudioSource;
	}
}
