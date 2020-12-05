using System;
using UnityEngine;

public class Target : MonoBehaviour
{
	public event Action<Entity> OnHit;

	private Entity _entity;

	private void Awake()
	{
		_entity = GetComponent<Entity>();
	}

	public void Hit()
	{
		// Debug.Log("Hit -> " + name);
		OnHit?.Invoke(_entity);
	}

	public void Activate(bool value)
	{
		_entity.BodyCollider.gameObject.SetActive(value);
	}
}

