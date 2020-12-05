using UnityEngine;

public class Renderer : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _bodyRenderer;
	[SerializeField] private SpriteRenderer _coreRenderer;

	[HideInInspector] public Color Color;

	private void Update()
	{
		_coreRenderer.color = Color;
	}

	public void SetSize(Size size)
	{
		switch (size)
		{
			case Size.Normal:
				_bodyRenderer.transform.localScale = new Vector2(1f, 1f);
				break;
			default:
				_bodyRenderer.transform.localScale = new Vector2(0.80f, 0.80f);
				break;
		}
	}

	public enum Size { Normal, Small }
}
