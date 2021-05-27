using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Celeritas.UI
{
	public class Tooltip : Singleton<Tooltip>
	{
		[SerializeField, Title("Assignments")]
		private RectTransform background;

		[SerializeField]
		private TextMeshProUGUI title;

		[SerializeField]
		private TextMeshProUGUI subtitle;

		[SerializeField]
		private TextMeshProUGUI description;

		[SerializeField]
		private GameObject textrow;

		[SerializeField, PropertySpace(0, 20)]
		private RectTransform parent;

		[SerializeField, FoldoutGroup("Subheaders")]
		private Transform effectSubheader;

		[SerializeField, FoldoutGroup("Subheaders")]
		private Transform shipSubheader;

		[SerializeField, FoldoutGroup("Subheaders")]
		private Transform weaponSubheader;

		[SerializeField, FoldoutGroup("Subheaders")]
		private Transform projectileSubheader;

		/// <summary>
		/// Is the tooltip currently being shown to the user.
		/// </summary>
		public bool IsShowing { get; private set; }

		protected override void Awake()
		{
			CleanupChildren();

			base.Awake();
		}

		/// <summary>
		/// Show a tooltip with the information provided within the entity.
		/// </summary>
		/// <param name="entity">The entity to present in the tooltip.</param>
		public void Show(Entity entity)
		{
			IsShowing = true;
			CleanupChildren();

			title.text = entity.Data.Title;

			if (entity is ModuleEntity module)
			{
				description.text = $"{module.ModuleData.Description}";
				subtitle.text = $"{module.ModuleData.ModuleCatagory} - {module.ModuleData.ModuleSize} - Level {module.Level}";

				CreateEffectRows(entity.EntityEffects, effectSubheader);

				if (module.HasShipEffects)
					CreateEffectRows(module.ShipEffects, shipSubheader);

				if (module.HasShipWeaponEffects)
					CreateEffectRows(module.ShipWeaponEffects, weaponSubheader);

				if (module.HasShipProjectileEffects)
					CreateEffectRows(module.ShipProjectileEffects, projectileSubheader);
			}
			else
			{
				description.text = "<i>Not Implemented</i>";
				subtitle.text = $"<i>Not Implemented</i>";

				CreateEffectRows(entity.EntityEffects, effectSubheader);
			}

			background.gameObject.SetActive(true);
			RebuildLayout();

			var pos = Mouse.current.position.ReadValue();
			pos.x -= background.rect.width / 2;
			background.transform.position = pos;
		}

		/// <summary>
		/// Hide this tooltip.
		/// </summary>
		public void Hide()
		{
			IsShowing = false;
			background.gameObject.SetActive(false);
			CleanupChildren();
		}

		private void CleanupChildren()
		{
			effectSubheader.gameObject.SetActive(false);
			shipSubheader.gameObject.SetActive(false);
			weaponSubheader.gameObject.SetActive(false);
			projectileSubheader.gameObject.SetActive(false);

			for (int i = 0; i < parent.childCount; i++)
			{
				var child = parent.GetChild(i);

				if (!IsSubheader(child))
				{
					Destroy(child.gameObject);
					i--;
				}
			}
		}

		private bool IsSubheader(Transform value)
		{
			return value == effectSubheader
				|| value == shipSubheader
				|| value == weaponSubheader
				|| value == projectileSubheader;
		}

		private void CreateEffectRows(EffectManager effects, Transform subheader)
		{
			CreateEffectRows(effects.EffectWrapperCopy, subheader);
		}

		private void CreateEffectRows(EffectWrapper[] effects, Transform subheader)
		{
			if (effects.Length == 0)
				return;

			subheader.gameObject.SetActive(true);
			Transform sibling = subheader;

			foreach (var effect in effects)
			{
				foreach (var wrapper in effect.EffectCollection.Systems)
				{
					var row = Instantiate(textrow, parent).transform;
					row.SetSiblingIndex(sibling.GetSiblingIndex() + 1);
					sibling = row;

					row.GetComponent<TextMeshProUGUI>().text = $" {wrapper.GetTooltip(effect.Level)}";
				}
			}
		}

		private void RebuildLayout()
		{
			for (int i = 0; i < parent.childCount; i++)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(parent.GetChild(i).GetComponent<RectTransform>());
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
			LayoutRebuilder.ForceRebuildLayoutImmediate(background);
		}
	}
}