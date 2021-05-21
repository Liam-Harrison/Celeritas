using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace Celeritas.UI
{
	/// <summary>
	/// An indivudal module button in the UI.
	/// </summary>
	public class ModuleSelectionItem : MonoBehaviour
	{
		[SerializeField, Title("Assignments")]
		private Button button;

		[SerializeField]
		private TextMeshProUGUI label;

		[SerializeField]
		private Image image;

		/// <summary>
		/// The button attatched to this gameobject.
		/// </summary>
		public Button Button { get => button; }

		/// <summary>
		/// The label attatched to this gameobject.
		/// </summary>
		public string Label { get => label.text; set => label.text = value; }

		/// <summary>
		/// The image attatched to this gameobject.
		/// </summary>
		public Image Image { get => image; }
	}
}