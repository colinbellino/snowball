using UnityEngine;

public static class Helpers
{
	public static T RandomItem<T>(T[] list) => list[Random.Range(0, list.Length)];
}
