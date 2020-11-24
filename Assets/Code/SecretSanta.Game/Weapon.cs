using UnityEngine;

public class Weapon : MonoBehaviour
{
	[SerializeField] private Entity _projectilePrefab;
	[SerializeField] private Transform _projectileOrigin;

	private double _cooldownTimestamp;
	private Entity _entity;

	private void Awake()
	{
		_entity = GetComponent<Entity>();
	}

	public void Fire()
	{
		if (_cooldownTimestamp > Time.time) { return; }

		var projectileEntity = GameObject.Instantiate(_projectilePrefab, _projectileOrigin.position, _projectileOrigin.rotation);
		projectileEntity.Alliance.SetCurrent(_entity.Alliance.Current);

		_cooldownTimestamp = Time.time + projectileEntity.Projectile.Cooldown;
	}
}
