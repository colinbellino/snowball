using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Snowball.Game
{
	public class WorldmapEncounterPoint : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField] private SpriteRenderer _spriteRenderer;
		private bool _enabled;

		public event Action Clicked;

		public void OnPointerClick(PointerEventData eventData)
		{
			if (_enabled)
			{
				Clicked?.Invoke();
			}
		}

		public void SetEnabled(bool enabled)
		{
			_enabled = enabled;
			_spriteRenderer.color = _enabled ? new Color(0.4f, 0.8f, 0.2f, 1f) : Color.gray;
		}
	}
}
