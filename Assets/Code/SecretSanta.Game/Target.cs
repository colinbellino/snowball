using UnityEngine;

public class Target : MonoBehaviour
{
	private Entity _entity;

	private void Awake()
	{
		_entity = GetComponent<Entity>();
	}

	public void Hit()
	{
		Debug.Log("Hit -> " + name);
		Destroy(gameObject);
		// GameManager.EntityDestroyed?.Invoke(_entity);
	}
}

