using Celeritas.Extensions;
using Celeritas.Game;
using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections;
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
		private CanvasGroup group;

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

		[SerializeField, PropertyRange(0, 1)]
		private float fadeTime = 0.25f;

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

		private bool thisShowing = false;

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

					if (hull.TryGetModuleEntity(grid.x, grid.y, out var module))
					{
						over = module;

						if (!hovering)
							hovered = Time.time;
						else
							Show(over);

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
			}
			else if (Physics.Raycast(ray, out hit, 20f))
			{
				var entity = hit.collider.GetComponentInParent<Entity>();
				if (entity != null && entity is ModuleEntity module)
				{
					over = module;

					if (!hovering)
						hovered = Time.time;
					else
						Show(over);

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

			if (hovering && Time.time > hovered + waitTime && !thisShowing && !IsShowing && over != null)
			{
				Show(over);
				thisShowing = true;
			}
			else if (!hovering && Time.time > hovered + waitTime && thisShowing && IsShowing)
			{
				thisShowing = false;
				Hide();
			}

			if (IsShowing)
			{
				var pos = Mouse.current.position.ReadValue();
				pos.y -= background.rect.height / 2;
				background.transform.position = Vector3.Lerp(background.transform.position, pos, 0.98f);
			}
		}

		/// <summary>
		/// Show a tooltip with the information provided within the entity.
		/// </summary>
		/// <param name="entity">The entity to present in the tooltip.</param>
		public void Show(ModuleEntity entity)
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

			if (!background.gameObject.activeInHierarchy)
			{
				var pos = Mouse.current.position.ReadValue();
				pos.y -= background.rect.height / 2;
				background.transform.position = pos;

				StopAllCoroutines();
				StartCoroutine(FadeIn());
			}

			RebuildLayout();
		}

		/// <summary>
		/// Hide this tooltip.
		/// </summary>
		public void Hide()
		{
			if (!IsShowing)
				return;

			IsShowing = false;
			StopAllCoroutines();
			StartCoroutine(FadeOut());
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
			foreach (var rect in parent.GetComponentsInChildren<RectTransform>())
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
			LayoutRebuilder.ForceRebuildLayoutImmediate(background);

			Canvas.ForceUpdateCanvases();
		}

		private IEnumerator FadeIn()
		{
			group.alpha = 0;

			background.gameObject.SetActive(true);
			RebuildLayout();

			yield return null;

			background.gameObject.SetActive(false);
			RebuildLayout();

			yield return null;

			background.gameObject.SetActive(true);
			RebuildLayout();

			float started = Time.unscaledTime;
			float p;

			do
			{
				p = Mathf.Clamp01((Time.unscaledTime - started) / fadeTime);
				group.alpha = p;
				yield return null;
			} while (p < 1f);

			yield break;
		}

		private IEnumerator FadeOut()
		{
			float started = Time.unscaledTime;
			float p;

			do
			{
				p = Mathf.Clamp01((Time.unscaledTime - started) / fadeTime);
				group.alpha = 1f - p;
				yield return null;
			} while (p < 1f);

			background.gameObject.SetActive(false);
			CleanupChildren();
			yield break;
		}
	}
}