using System;
using UnityEngine;

public class Target : MonoBehaviour
{
	public event Action<Entity> OnKilled;

	private Entity _entity;

	private void Awake()
	{
		_entity = GetComponent<Entity>();
	}

	public void Hit()
	{
		Debug.Log("Hit -> " + name);
		OnKilled(_entity);
		Destroy(gameObject);
	}
}

