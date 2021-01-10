using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Snowball.Game
{
	public class WorldmapEncounterPoint : MonoBehaviour, IPointerClickHandler
	{
		public event Action Clicked;

		public void OnPointerClick(PointerEventData eventData)
		{
			Clicked?.Invoke();
		}
	}
}
