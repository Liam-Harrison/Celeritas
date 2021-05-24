using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Celeritas.UI
{
	public class TooltipUI : MonoBehaviour
	{
		[SerializeField, Title("Assignments")]
		private TextMeshProUGUI title;

		[SerializeField]
		private TextMeshProUGUI subtitle;

		[SerializeField]
		private TextMeshProUGUI description;

		[SerializeField]
		private GameObject spacePrefab;

		[SerializeField]
		private GameObject tooltipRowPrefab;
	}
}