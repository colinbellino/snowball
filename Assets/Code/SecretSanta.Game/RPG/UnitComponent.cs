using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class UnitComponent : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _bodyRenderer;

		public void UpdateData(Unit data)
		{
			SetName(data.Name);
			_bodyRenderer.color = data.Color;
		}

		public void SetGridPosition(Vector2Int position)
		{
			transform.position = new Vector3(position.x, position.y, 0f);
		}

		private void SetName(string value)
		{
			name = $"Unit [{value}]";
		}
	}
}
