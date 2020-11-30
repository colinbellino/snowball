using UnityEngine;

public class BarkManager
{
	public Bark GetRandomBark(BarkType type, Recruit recruit)
	{
		var barks = new Bark[0];

		switch (type)
		{
			case BarkType.Died:
				barks = recruit.Barks.Died;
				break;
			case BarkType.Recruited:
				barks = recruit.Barks.Recruited;
				break;
			case BarkType.AllyDied:
				barks = recruit.Barks.AllyDied;
				break;
			case BarkType.LevelFinished:
				barks = recruit.Barks.LevelFinished;
				break;
			default:
				Debug.LogError("Unknown bark type: " + type);
				break;
		}

		if (barks.Length == 0)
		{
			return null;
		}

		var randomIndex = Random.Range(0, barks.Length);
		return barks[randomIndex];
	}
}

public enum BarkType { Recruited, Died, AllyDied, LevelFinished }
