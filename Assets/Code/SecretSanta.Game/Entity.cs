using UnityEngine;

public class Entity : MonoBehaviour
{
	[SerializeField] private Collider2D _bodyCollider;

	public Transform Transform => transform;
	public Weapon Weapon { get; private set; }
	public Movement Movement { get; private set; }
	public Target Target { get; private set; }
	public Alliance Alliance { get; private set; }
	public Projectile Projectile { get; private set; }
	public Renderer Renderer { get; private set; }
	public Brain Brain { get; private set; }
	public Collider2D BodyCollider => _bodyCollider;

	private void Awake()
	{
		Weapon = GetComponent<Weapon>();
		Movement = GetComponent<Movement>();
		Target = GetComponent<Target>();
		Alliance = GetComponent<Alliance>();
		Projectile = GetComponent<Projectile>();
		Renderer = GetComponent<Renderer>();
		Brain = GetComponent<Brain>();
	}
}
