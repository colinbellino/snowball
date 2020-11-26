using UnityEngine;

[CreateAssetMenu(menuName = "Secret Santa/Config")]
public class GameConfig : ScriptableObject
{
	public Recruit[] Recruits;
	public Level[] Levels;
	public Entity EnemyPrefab;
	public Entity RecruitPrefab;
	public readonly Vector2 AreaSize = new Vector2(32, 18);
}
