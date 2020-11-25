using UnityEngine;

[CreateAssetMenu(menuName = "SecretSanta/Recruit")]
public class Recruit : ScriptableObject
{
	public string Id;
	public Color Color = Color.magenta;
	public float MoveSpeed;

	[HideInInspector] public string Name;
}
