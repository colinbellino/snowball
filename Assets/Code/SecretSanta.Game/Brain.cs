using UnityEngine;

public class Brain : MonoBehaviour
{
	[SerializeField] private float _fireDelay = 0.5f;
	// [SerializeField] private Aim _aimType;

	private Entity _entity;
	private double _fireCooldownTimestamp;

	private void Awake()
	{
		_entity = GetComponent<Entity>();
	}

	private void Update()
	{
		var moveDirection = new Vector2(0, -0.1f);
		_entity.Movement.MoveInDirection(moveDirection);
		_entity.Movement.RotateInDirection(moveDirection);

		var bounds = new Bounds(Vector3.zero, GameManager.AreaSize);
		if (bounds.Contains(transform.position))
		{
			if (Time.time >= _fireCooldownTimestamp)
			{
				_entity.Weapon.Fire();
				_fireCooldownTimestamp = Time.time + _fireDelay;
			}
		}
	}
}

// public enum Aim { Direction, Target }
