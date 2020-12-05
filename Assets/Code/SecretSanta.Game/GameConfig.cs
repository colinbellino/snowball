using UnityEngine;

[CreateAssetMenu(menuName = "Secret Santa/Config")]
public class GameConfig : ScriptableObject
{
	[Header("Cheats")]
	public bool CheatsFullTeam = false;

	[Header("Data")]
	public Recruit[] Recruits;
	public Level[] Levels;
	public string[] PlaceholderNames = { "Anri", "Rena", "Riku", "Andre", "Thea", "Mariel", "Jesse", "Marceline", "Gaius", "Ursula", "Madeline", "Theo", "Régis" };

	[Header("Prefabs")]
	public Entity EnemyPrefab;
	public Entity RecruitPrefab;

	public readonly Vector2 AreaSize = new Vector2(32, 18);

}
