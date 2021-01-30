using UnityEngine;
using UnityEngine.UI;

namespace Snowball.Game
{
	public class DebugUI : MonoBehaviour
	{
		[SerializeField] private Text _versionText;

		private void Awake()
		{
			_versionText.text = $"v{Application.version}";
		}
	}
}
