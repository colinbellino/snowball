using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private float _speed = 10f;

	public Transform Transform => transform;
	public float Speed => _speed;
}
