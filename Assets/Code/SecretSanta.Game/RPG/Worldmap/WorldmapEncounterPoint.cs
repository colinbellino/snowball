using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.SecretSanta.Game.RPG
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
