using UnityEngine;

public class Movement : MonoBehaviour
{
	[SerializeField] private float _speed = 10f;

	public void MoveInDirection(Vector2 direction, bool stayInBounds = false)
	{
		var size = transform.localScale;
		var areaSize = GameManager.AreaSize;

		var position = transform.position + (Vector3) direction * (Time.deltaTime * _speed);
		if (stayInBounds)
		{
			position.x = Mathf.Clamp(position.x, -areaSize.x / 2f + size.x / 2f, areaSize.x / 2f - size.x / 2f);
			position.y = Mathf.Clamp(position.y, -areaSize.y / 2f + size.y / 2f, areaSize.y / 2f - size.y / 2f);
		}

		transform.position = position;
	}

	public void RotateInDirection(Vector2 direction)
	{
		transform.up = direction;
	}
}
