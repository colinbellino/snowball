#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;

namespace Code.SecretSanta.Game.RPG
{
	public class UnitDebugger : MonoBehaviour
	{
		[SerializeField] private Canvas _canvas;
		[SerializeField] private Text _debugText;

		private Unit _unit;

		private void Update()
		{
			if (_unit == null)
			{
				_unit = Game.Instance.Battle.SortedUnits.Find(unit => unit.Name == gameObject.name);
				return;
			}

			if (_unit.HealthCurrent > 0)
			{
				SetDebugText($"{_unit.Name} \n [{_unit.GridPosition.x},{_unit.GridPosition.y}] · {_unit.HealthCurrent}/{_unit.HealthMax}");
			}
			else
			{
				_canvas.gameObject.SetActive(false);
			}
		}

		private void SetDebugText(string value)
		{
			_debugText.text = value;
		}
	}
}
#endif
