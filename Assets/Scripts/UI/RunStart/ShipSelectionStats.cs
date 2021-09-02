using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.RunStart
{
	/// <summary>
	/// Makes it easy to access and change elements of the ship's stat preview
	/// One will exist for each stat. Eg, one for health, one for speed, ect
	/// </summary>
	class ShipSelectionStats : MonoBehaviour
	{
		[SerializeField]
		public TextMeshProUGUI title;

		[SerializeField]
		public Slider slider;

	}
}
