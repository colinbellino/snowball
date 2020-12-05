using UnityEngine;

public class Brain : MonoBehaviour
{
	[HideInInspector] public float FireDelay;

	private Entity _entity;
	private double _fireCooldownTimestamp;

	private void Awake()
	{
		_entity = GetComponent<Entity>();
	}

	public void Tick()
	{
		var moveDirection = transform.up * 0.1f;
		_entity.Movement.MoveInDirection(moveDirection);
		// _entity.Movement.RotateInDirection(moveDirection);

		var bounds = new Bounds(Vector3.zero, GameManager.Instance.Config.AreaSize);
		if (bounds.Contains(transform.position))
		{
			if (Time.time >= _fireCooldownTimestamp)
			{
				_entity.Weapon.Fire();
				_fireCooldownTimestamp = Time.time + FireDelay;
			}
		}
	}
}
