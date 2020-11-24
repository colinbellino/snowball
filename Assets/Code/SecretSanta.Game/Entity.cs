using UnityEngine;

public class Entity : MonoBehaviour
{
	public Transform Transform => transform;
	public Weapon Weapon { get; private set; }
	public Movement Movement { get; private set; }
	public Target Target { get; private set; }
	public Alliance Alliance { get; private set; }
	public Projectile Projectile { get; private set; }

	private void Awake()
	{
		Weapon = GetComponent<Weapon>();
		Movement = GetComponent<Movement>();
		Target = GetComponent<Target>();
		Alliance = GetComponent<Alliance>();
		Projectile = GetComponent<Projectile>();
	}
}
