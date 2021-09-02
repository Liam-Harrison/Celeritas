using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
