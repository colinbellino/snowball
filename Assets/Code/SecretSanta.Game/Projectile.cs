using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private float _speed = 10f;
	[SerializeField] private float _cooldown = 0.15f;

	public float Cooldown => _cooldown;

	private void Update()
	{
		transform.position += transform.up * (Time.deltaTime * _speed);
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		var target = collider.GetComponentInParent<Target>();
		target?.Hit();
	}
}
