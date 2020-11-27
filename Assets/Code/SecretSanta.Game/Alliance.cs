using UnityEngine;

public class Alliance : MonoBehaviour
{
	[SerializeField] private AllianceType _current;

	public AllianceType Current => _current;
	private Entity _entity;

	private void Awake()
	{
		_entity = GetComponent<Entity>();
	}

	private void Update()
	{
		_entity.BodyCollider.gameObject.layer = AllianceToLayer(_entity.Alliance.Current);
	}

	public void SetCurrent(AllianceType alliance)
	{
		_current = alliance;
	}

	public static int AllianceToLayer(AllianceType alliance)
	{
		return alliance == AllianceType.Ally
			? LayerMask.NameToLayer("Ally")
			: LayerMask.NameToLayer("Enemy");
	}
}

public enum AllianceType { Ally, Enemy }
