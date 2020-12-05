using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	[SerializeField] private Entity _projectilePrefab;
	[SerializeField] private Transform _projectileOrigin;

	private Entity _entity;
	private double _cooldownTimestamp;

	public event Action<Entity> OnFired;

	private void Awake()
	{
		_entity = GetComponent<Entity>();
	}

	public void Fire()
	{
		if (_cooldownTimestamp > Time.time) { return; }

		var projectileEntity = Instantiate(_projectilePrefab, _projectileOrigin.position, _projectileOrigin.rotation);
		projectileEntity.Alliance.SetCurrent(_entity.Alliance.Current);
		projectileEntity.Renderer.Color = _entity.Renderer.Color;
		OnFired?.Invoke(projectileEntity);

		_cooldownTimestamp = Time.time + projectileEntity.Projectile.Cooldown;
	}
}
