using UnityEngine;
using UnityEngine.UI;

namespace Snowball.Game
{
	public class UnitDebugger : MonoBehaviour
	{
		[SerializeField] private Canvas _canvas;
		[SerializeField] private Text _debugText;

		private Unit _unit;

		private void Awake()
		{
			_canvas.gameObject.SetActive(false);
		}

		public void Init(Unit unit)
		{
			_unit = unit;
		}

		private void Update()
		{
			if (_unit == null)
			{
				return;
			}

			if (_unit.HealthCurrent > 0)
			{
				_canvas.gameObject.SetActive(true);
				SetDebugText($"{_unit.Name}\n[{_unit.GridPosition.x},{_unit.GridPosition.y}]\nHP {_unit.HealthCurrent}/{_unit.HealthMax} · CT {_unit.ChargeTime}");
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
