using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private float _speed = 10f;
	[SerializeField] private float _cooldown = 0.15f;

	private Entity _entity;

	public float Cooldown => _cooldown;

	public static event Action<Entity> OnImpact;

	private void Awake()
	{
		_entity = GetComponent<Entity>();
	}

	private void Update()
	{
		transform.position += transform.up * (Time.deltaTime * _speed);

		var bounds = new Bounds(Vector3.zero, GameManager.Instance.Config.AreaSize);
		if (bounds.Contains(transform.position) == false)
		{
			OnImpact?.Invoke(_entity);
		}
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		var target = collider.GetComponentInParent<Target>();
		target?.Hit();
	}
}
