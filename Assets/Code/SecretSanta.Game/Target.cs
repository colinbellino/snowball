using System;
using UnityEngine;

public class Target : MonoBehaviour
{
	public event Action<Entity> OnDestroyed;

	private Entity _entity;

	private void Awake()
	{
		_entity = GetComponent<Entity>();
	}

	public void Hit()
	{
		// Debug.Log("Hit -> " + name);
		OnDestroyed(_entity);
		Destroy(gameObject);
	}
}

