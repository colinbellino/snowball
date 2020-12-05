using UnityEngine;

public class GameUI : MonoBehaviour
{
	public RecruitmentUI Recruitment { get; private set; }
	public GameplayUI Gameplay { get; private set; }

	private void Awake()
	{
		Recruitment = FindObjectOfType<RecruitmentUI>();
		Gameplay = FindObjectOfType<GameplayUI>();
	}
}
