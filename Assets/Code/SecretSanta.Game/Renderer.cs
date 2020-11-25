using UnityEngine;

public class Renderer : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _bodyRenderer;
	[SerializeField] private SpriteRenderer _coreRenderer;

	public Color Color;

	private void Update()
	{
		_coreRenderer.color = Color;
	}
}
