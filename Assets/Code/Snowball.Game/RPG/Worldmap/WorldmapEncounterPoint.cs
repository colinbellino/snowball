using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Snowball.Game
{
	public class WorldmapEncounterPoint : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] private SpriteRenderer _spriteRenderer;
		[SerializeField] private Canvas _canvas;
		[SerializeField] private Text _text;

		public event Action Clicked;

		private void Awake()
		{
			_canvas.gameObject.SetActive(false);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_canvas.gameObject.SetActive(true);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_canvas.gameObject.SetActive(false);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			Clicked?.Invoke();
		}

		public void SetCleared(EncounterAuthoring encounter, bool cleared)
		{
			var color = cleared ? new Color(0.2f, 0.6f, 0.2f, 1f) : new Color(0.8f, 0.2f, 0.2f, 1f);
			_spriteRenderer.material.SetColor("ReplacementColor1", color);
			_text.text = encounter.Name;
		}
	}
}
