using Celeritas.Extensions;
using Celeritas.Game;
using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Celeritas.UI
{
	public class Tooltip : Singleton<Tooltip>
	{
		private class TooltipRequest
		{
			public GameObject requester;
			public ModuleEntity entity;

			public TooltipRequest(GameObject requester, ModuleEntity entity)
			{
				this.requester = requester;
				this.entity = entity;
			}
		}

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

		[SerializeField, PropertyRange(0, 12)]
		private float padding = 4f;

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
		public Entity Showing { get => requested.entity; }

		protected override void Awake()
		{
			CleanupChildren();
			base.Awake();
		}

		private void Update()
		{
			if (IsShowing)
			{
				var pos = Mouse.current.position.ReadValue();
				var halfWidth = background.sizeDelta.x / 2f;
				var halfHeight = background.sizeDelta.y / 2f;

				pos.y -= halfHeight;

				if (pos.x + halfWidth + padding > Screen.width)
					pos.x -= (pos.x + halfWidth + padding) - Screen.width;

				background.transform.position = Vector3.Lerp(background.transform.position, pos, 0.95f);
			}
		}

		private readonly List<TooltipRequest> requests = new List<TooltipRequest>();
		private TooltipRequest requested;

		/// <summary>
		/// Show a tooltip with the information provided within the entity.
		/// </summary>
		/// <param name="entity">The entity to present in the tooltip.</param>
		public void RequestShow(GameObject requester, ModuleEntity entity)
		{
			if (requested != null && requested.requester == requester)
				ShowTooltip(new TooltipRequest(requester, entity));
			if (requested != null && !HasRequester(requester))
				requests.Add(new TooltipRequest(requester, entity));
			else
				ShowTooltip(new TooltipRequest(requester, entity));
		}

		private void ShowTooltip(TooltipRequest request)
		{
			requested = request;
			IsShowing = true;

			CleanupChildren();

			var module = request.entity;
			title.text = module.Data.Title;

			description.text = $"{module.ModuleData.Description}";
			subtitle.text = $"{module.ModuleData.ModuleCatagory} - {module.ModuleData.ModuleSize} - Level {module.Level}";

			CreateEffectRows(module.EntityEffects, effectSubheader);

			if (module.HasShipEffects)
				CreateEffectRows(module.ShipEffects, shipSubheader);

			if (module.HasShipWeaponEffects)
				CreateEffectRows(module.ShipWeaponEffects, weaponSubheader);

			if (module.HasShipProjectileEffects)
				CreateEffectRows(module.ShipProjectileEffects, projectileSubheader);

			var pos = Mouse.current.position.ReadValue();
			pos.y -= background.rect.height / 2;
			background.transform.position = pos;

			if (!background.gameObject.activeInHierarchy)
			{
				StopAllCoroutines();
				StartCoroutine(FadeIn());
			}

			StartCoroutine(RebuildLayout());
		}

		/// <summary>
		/// Hide this tooltip.
		/// </summary>
		public void ReleaseRequest(GameObject requester)
		{
			if (requested == null || !HasRequester(requester))
				return;

			if (requests.Count > 0)
			{
				for (int i = 0; i < requests.Count; i++)
				{
					if (requests[i].requester == requester)
					{
						requests.RemoveAt(i);
						break;
					}
				}

				if (requests.Count > 0)
				{
					if (requested.requester == requester)
					{
						ShowTooltip(requests[0]);
						requests.RemoveAt(0);
					}
				}
				else
				{
					HideTooltip();
				}
			}
			else
			{
				HideTooltip();
			}
		}

		private bool HasRequester(GameObject requester)
		{
			if (requested.requester == requester)
				return true;

			foreach (var request in requests)
			{
				if (request.requester == requester)
					return true;
			}

			return false;
		}

		private void HideTooltip()
		{
			requested = null;
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

		private IEnumerator RebuildLayout()
		{
			Canvas.ForceUpdateCanvases();

			yield return null;

			foreach (var rect in parent.GetComponentsInChildren<RectTransform>())
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
			}

			yield return null;

			LayoutRebuilder.ForceRebuildLayoutImmediate(parent);

			yield return null;

			LayoutRebuilder.ForceRebuildLayoutImmediate(background);

			yield return null;

			Canvas.ForceUpdateCanvases();

			yield return null;

			yield break;
		}

		private IEnumerator FadeIn()
		{
			group.alpha = 0;
			background.gameObject.SetActive(true);

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

			while (Time.unscaledTime < started + waitTime)
				yield return null;

			started = Time.unscaledTime;
			float p;

			if (requested != null)
				yield break;

			do
			{
				p = Mathf.Clamp01((Time.unscaledTime - started) / fadeTime);
				group.alpha = 1f - p;
				yield return null;
			} while (p < 1f);

			background.gameObject.SetActive(false);
			CleanupChildren();
			requested = null;

			IsShowing = false;
			yield break;
		}
	}
}