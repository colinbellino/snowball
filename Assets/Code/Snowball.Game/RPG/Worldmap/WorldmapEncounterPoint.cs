using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Snowball.Game
{
	public class WorldmapEncounterPoint : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField] private SpriteRenderer _spriteRenderer;

		public event Action Clicked;

		public void OnPointerClick(PointerEventData eventData)
		{
				Clicked?.Invoke();
		}

		public void SetCleared(bool cleared)
		{
			var color = cleared ? new Color(0.2f, 0.6f, 0.2f, 1f) : new Color(0.8f, 0.2f, 0.2f, 1f);
			_spriteRenderer.material.SetColor("ReplacementColor1", color);
		}
	}
}
