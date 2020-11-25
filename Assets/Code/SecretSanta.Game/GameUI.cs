using UnityEngine;

public class GameUI : MonoBehaviour
{
	public RecruitmentUI Recruitment { get; private set; }

	private void Awake()
	{
		Recruitment = FindObjectOfType<RecruitmentUI>();
	}
}
