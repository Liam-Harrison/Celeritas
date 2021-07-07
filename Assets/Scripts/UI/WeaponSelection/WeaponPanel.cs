using Celeritas.Game;
using Celeritas.Scriptables;
using Celeritas.UI.General;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeritas.UI.WeaponSelection
{
	/// <summary>
	/// Manages UI elements for a weapon panel.
	/// </summary>
	public class WeaponPanel : MonoBehaviour
	{
		[SerializeField, TitleGroup("Assignments")]
		private TextMeshProUGUI header;

		[SerializeField, TitleGroup("Assignments")]
		private TextMeshProUGUI subheader;

		[SerializeField, TitleGroup("Assignments")]
		private TextMeshProUGUI description;

		[SerializeField, TitleGroup("Assignments")]
		private Image background;

		[SerializeField, TitleGroup("Assignments")]
		private IconUI icon;

		[SerializeField, TitleGroup("Assignments")]
		private LineUI line;

		[SerializeField, TitleGroup("Assignments")]
		private RectTransform leftAnchor;

		[SerializeField, TitleGroup("Assignments")]
		private RectTransform rightAnchor;

		[SerializeField, TitleGroup("Colors")]
		private Color normal;

		/// <summary>
		/// The rect transform of this panel.
		/// </summary>
		public RectTransform RectTransform { get; private set; }

		/// <summary>
		/// The module attatched to this panel.
		/// </summary>
		public Module Module { get; private set; }

		/// <summary>
		/// The weapon set on this panel.
		/// </summary>
		public WeaponData Weapon { get; private set; }

		private void Awake()
		{
			RectTransform = GetComponent<RectTransform>();
		}

		/// <summary>
		/// Set the weapon of this panel to the speicifed weapon.
		/// </summary>
		/// <param name="weapon">The weapon to set.</param>
		public void SetWeapon(WeaponData weapon)
		{
			Weapon = weapon;

			if (weapon != null)
			{
				header.text = weapon.Title;
				subheader.text = weapon.EntityInstance.Subheader;
				description.text = weapon.Description;
				icon.SetItem(weapon);

				if (Module != null)
					Module.SetModule(weapon);
			}
		}

		/// <summary>
		/// Set the module of this panel to the specified module.
		/// </summary>
		/// <param name="module">The module to attatch to.</param>
		public void SetModule(Module module)
		{
			Module = module;

			if (module != null)
			{
				line.WorldTarget = module.transform;
				var d = RectTransform.InverseTransformPoint(Camera.main.WorldToScreenPoint(module.transform.position));
				line.UITarget = d.x <= 0 ? leftAnchor : rightAnchor;

			}
			else
				line.WorldTarget = null;
		}
	}
}