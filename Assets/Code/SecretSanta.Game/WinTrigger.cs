using UnityEngine;

public class WinTrigger : MonoBehaviour
{
	private Color _color = new Color(1, 0, 0, 0.25f);

	void OnDrawGizmos()
	{
		Gizmos.color = _color;
		Gizmos.DrawCube(transform.position, transform.localScale);
	}
}
