using UnityEngine;

[CreateAssetMenu(menuName = "Secret Santa/Recruit")]
public class Recruit : ScriptableObject
{
	public string Id;
	public Color Color = Color.magenta;
	public float MoveSpeed;

	[HideInInspector] public string Name;
}
