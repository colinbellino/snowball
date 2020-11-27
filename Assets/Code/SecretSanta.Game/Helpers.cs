using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public static class Helpers
{
	public static T RandomItem<T>(T[] list) => list[Random.Range(0, list.Length)];

	public static bool IsPressed(InputAction inputAction)
	{
		return inputAction.ReadValue<float>() > 0f;
	}

	public static bool WasPressedThisFrame(InputAction inputAction)
	{
		return inputAction.triggered && inputAction.ReadValue<float>() > 0f;
	}

	public static bool WasReleasedThisFrame(InputAction inputAction)
	{
		if (inputAction.activeControl == null)
		{
			return false;
		}

		return ((ButtonControl) inputAction.activeControl).wasPressedThisFrame;
	}
}
