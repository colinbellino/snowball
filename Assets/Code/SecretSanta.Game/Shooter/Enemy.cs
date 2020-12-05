using UnityEngine;

[CreateAssetMenu(menuName = "Secret Santa/Enemy")]
public class Enemy : ScriptableObject
{
	public string Id => name;

	[Header("Renderer")]
	public Color Color = Color.magenta;

	[Header("Movement")]
	public float MoveSpeed = 10;

	[Header("Brain")]
	public float FireDelay = 1f;
}
