using Celeritas.Game;
using Celeritas.Game.Controllers;
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

		[SerializeField, PropertyRange(0, 1)]
		private float waitTime = 0.2f;

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
		public bool IsShowing { get; private set; } = false;

		/// <summary>
		/// The entity currently being shown.
		/// </summary>
		public Entity Showing { get; private set; }

		private new Camera camera;

		private ModuleEntity over;

		private float hovered = 0;

		private bool hovering = false;

		protected override void Awake()
		{
			camera = Camera.main;
			CleanupChildren();

			base.Awake();
		}

		private void Update()
		{
			if (GameStateManager.Instance.GameState == GameState.PLAY ||
				GameStateManager.Instance.GameState == GameState.BOSS)
				return;

			var ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
			RaycastHit hit;

			if (GameStateManager.Instance.GameState == GameState.BUILD)
			{
				if (Physics.Raycast(ray, out hit, 40f, LayerMask.GetMask("Hull")))
				{
					var hull = PlayerController.Instance.PlayerShipEntity.HullManager;
					var grid = hull.GetGridFromWorld(hit.point);

					if (hull.Modules[grid.x, grid.y].HasModuleAttatched)
					{
						var module = hull.Modules[grid.x, grid.y].AttatchedModule;
						if (IsShowing && module != over)
							Show(module);

						over = hull.Modules[grid.x, grid.y].AttatchedModule;
						hovered = Time.time;
						hovering = true;
					}
				}
				else
				{
					hovering = false;
				}
			}
			else if (Physics.Raycast(ray, out hit, 20f))
			{
				var entity = hit.collider.GetComponentInParent<Entity>();
				if (entity != null && entity is ModuleEntity module)
				{
					if (IsShowing && module != over)
						Show(module);

					over = module;
					hovered = Time.time;
					hovering = true;
				}
				else
				{
					hovering = false;
				}
			}
			else
			{
				hovering = false;
			}

			if (Time.time > hovered + waitTime && !IsShowing && over != null && over != Showing)
			{
				Show(over);
			}
			else if (!hovering && Time.time > hovered + waitTime && IsShowing && Showing == over)
			{
				Hide();
			}
		}

		/// <summary>
		/// Show a tooltip with the information provided within the entity.
		/// </summary>
		/// <param name="entity">The entity to present in the tooltip.</param>
		public void Show(Entity entity)
		{
			IsShowing = true;
			Showing = entity;

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
			pos.y -= background.rect.height / 2;
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