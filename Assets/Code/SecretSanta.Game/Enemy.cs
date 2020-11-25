using UnityEngine;

[CreateAssetMenu(menuName = "SecretSanta/Enemy")]
public class Enemy : ScriptableObject
{
	public string Id;
	public Color Color = Color.magenta;
	public float MoveSpeed;
}
