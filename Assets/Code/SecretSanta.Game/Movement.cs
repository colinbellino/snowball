using Cysharp.Threading.Tasks;
using UnityEngine;

public class Movement : MonoBehaviour
{
	[HideInInspector] public float Speed;

	private Entity _entity;

	private void Awake()
	{
		_entity = GetComponent<Entity>();
	}

	public void MoveInDirection(Vector2 direction, bool stayInBounds = false)
	{
		var size = transform.localScale;
		var areaSize = GameManager.Instance.Config.AreaSize;

		var position = transform.position + (Vector3) direction * (Time.deltaTime * Speed);
		if (stayInBounds)
		{
			position.x = Mathf.Clamp(position.x, -areaSize.x / 2f + size.x / 2f, areaSize.x / 2f - size.x / 2f);
			position.y = Mathf.Clamp(position.y, -areaSize.y / 2f + size.y / 2f, areaSize.y / 2f - size.y / 2f);
		}

		transform.position = position;

		CheckForObstacles();
	}

	public void RotateInDirection(Vector2 direction)
	{
		transform.up = direction;
	}

	public void Follow(Vector3 target, float speed, float distance = 1f)
	{
		var direction = target - transform.position;
		if (direction.magnitude > distance)
		{
			transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
		}
	}

	public void TeleportTo(Vector3 position)
	{
		transform.position = position;
	}

	public async void MoveTo(Vector3 position, float speed)
	{
		while ((position - transform.position).magnitude > 0.1f)
		{
			transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime * speed);
			await UniTask.NextFrame();
		}
	}

	private void CheckForObstacles()
	{
		var layer = LayerMask.NameToLayer("Obstacles");
		var hits = Physics2D.CircleCastAll (
			transform.position, _entity.BodyCollider.radius,
			Vector3.zero, 0f,
			1 << layer
		);
		for (var i = 0; i < hits.Length; i++)
		{
			if (hits[i].collider)
			{
				_entity.Target.Hit();
			}
		}
	}
}
