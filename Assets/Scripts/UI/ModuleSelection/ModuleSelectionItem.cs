using Celeritas.Game.Actions;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.UI.Tooltips;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeritas.UI
{
	/// <summary>
	/// An indivudal module button in the UI.
	/// </summary>
	public class ModuleSelectionItem : MonoBehaviour, ITooltip
	{
		[SerializeField, Title("Assignments")]
		private Button button;

		[SerializeField]
		private TextMeshProUGUI label;

		[SerializeField]
		private Image image;

		/// <summary>
		/// The moudle attatched to this item.
		/// </summary>
		public ModuleData Module { get; set; }

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

		/// <inheritdoc/>
		public ModuleEntity TooltipEntity => (ModuleEntity) Module.EntityInstance;

		public GameAction TooltipAction => null;
	}
}