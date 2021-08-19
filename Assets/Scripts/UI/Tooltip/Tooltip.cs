using Celeritas.Game;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Celeritas.UI
{
	/// <summary>
	/// Manages the tooltips and schedules what tooltips can be seen at any time.
	/// </summary>
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
		private Image icon;

		[SerializeField]
		private Image tetrisIcon;

		[SerializeField]
		private GameObject seperator;

		[SerializeField]
		private GameObject textrow;

		[SerializeField, PropertyRange(0, 1)]
		private float waitOutTime = 0.2f;

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

		public RectTransform RectTransform { get; private set; }

		protected override void Awake()
		{
			RectTransform = GetComponent<RectTransform>();
			CleanupChildren();
			base.Awake();
		}

		private void Update()
		{
			if (IsShowing)
			{
				var pos = GetPosition();

				background.position = Vector3.Lerp(background.position, pos, 24f * Time.smoothDeltaTime);
			}
		}

		private Vector3 GetPosition()
		{
			var m = Mouse.current.position.ReadValue();

			var s_rect = RectTransformToScreenSpace(background);
			var h_height = s_rect.height / 2f;
			var h_width = s_rect.width / 2f;

			var x = m.x;
			var y = m.y;

			y -= h_height + padding;
			x += h_width + padding;

			if (x + h_width + padding > Screen.width)
			{
				x = m.x - h_width - padding;
			}

			if (y - h_height - padding < 0)
			{
				y = m.y + h_height + padding;
			}

			return new Vector3(x, y);
		}

		private static Rect RectTransformToScreenSpace(RectTransform transform)
		{
			Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
			return new Rect((Vector2)transform.position - (size * 0.5f), size);
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

		private void ShowTooltip(TooltipRequest request)
		{
			requested = request;
			IsShowing = true;

			CleanupChildren();

			var module = request.entity;
			title.text = module.Data.Title;

			description.text = module.ModuleData.Description;
			subtitle.text = module.Subheader;

			if (module.ModuleData.Icon != null)
				icon.sprite = module.ModuleData.Icon;
			else
				icon.sprite = null;

			if (module.ModuleData.TetrisShape != TetrisShape.None)
				tetrisIcon.sprite = GameDataManager.Instance.GetTetrisSprite(module.ModuleData.TetrisShape);

			tetrisIcon.gameObject.SetActive(module.ModuleData.TetrisShape != TetrisShape.None);

			seperator.SetActive(false);

			CreateEffectRows(module.EntityEffects, effectSubheader);

			if (module.HasShipEffects)
				CreateEffectRows(module.ShipEffects, shipSubheader);

			if (module.HasShipWeaponEffects)
				CreateEffectRows(module.ShipWeaponEffects, weaponSubheader);

			if (module.HasShipProjectileEffects)
				CreateEffectRows(module.ShipProjectileEffects, projectileSubheader);

			if (module is WeaponEntity weapon)
				CreateEffectRows(weapon.ProjectileEffects, projectileSubheader);

			background.transform.position = GetPosition();

			if (!background.gameObject.activeInHierarchy)
			{
				StartCoroutine(FadeIn());
			}

			StartCoroutine(RebuildLayout());
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
					child.gameObject.SetActive(false);
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
					seperator.gameObject.SetActive(true);

					row.GetComponent<TextMeshProUGUI>().text = $"• <indent=5%> {wrapper.GetTooltip(effect.Level)}";
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

			background.transform.position = GetPosition();

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

			while (Time.unscaledTime < started + waitOutTime)
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