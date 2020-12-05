using System;
using UnityEngine;

public static class GameEvents
{
	public static Action GameInitialized;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void Init()
	{
		GameInitialized = null;
	}
}
