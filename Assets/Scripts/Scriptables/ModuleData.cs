using UnityEngine;
using Celeritas.Extensions;
using Celeritas.Game.Entities;
using Sirenix.OdinInspector;
using AssetIcons;
using Celeritas.Game;
using Sirenix.Utilities;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Contains the instanced information for a module.
	/// </summary>
	[CreateAssetMenu(fileName = "New Module", menuName = "Celeritas/New Module", order = 20)]
	public class ModuleData : EntityData
	{
		[HorizontalGroup("Module Info", Width = 50)]
		[BoxGroup("Module Info/Icon")]
		[SerializeField, PreviewField, HideLabel]
		[AssetIcon(maxSize: 50)]
		private Sprite icon;

		[HorizontalGroup("Module Info", Width = 50)]
		[BoxGroup("Module Info/Background")]
		[SerializeField, PreviewField, HideLabel]
		[AssetIcon(layer: -1)]
		private Sprite background;

		[HorizontalGroup("Module Info")]
		[BoxGroup("Module Info/Description")]
		[SerializeField, TextArea, HideLabel]
		private string description;

		[SerializeField]
		private ModuleSize size;

		[SerializeField, Title("Ship Effects")]
		private bool hasShipEffects;

		[SerializeField, ShowIf(nameof(hasShipEffects))]
		private EffectWrapper[] shipEffects;

		[SerializeField, Title("Weapon Effects")]
		private bool hasWeaponEffects;

		[SerializeField, ShowIf(nameof(hasWeaponEffects))]
		private EffectWrapper[] weaponEffects;

		[SerializeField, Title("Projectile Effects")]
		private bool hasProjectileEffects;

		[SerializeField, ShowIf(nameof(hasProjectileEffects))]
		private EffectWrapper[] projectileEffects;

		[SerializeField, Title("Module Layout")]
		[TableMatrix(SquareCells = true, DrawElementMethod = nameof(OnLayoutDraw))]
		private bool[,] moduleLayout = new bool[BaseLayoutResolution, BaseLayoutResolution];

		private static int BaseLayoutResolution = 3;

		public bool[,] ModuleLayout { get => moduleLayout;}

		/// <summary>
		/// The icon for the module.
		/// </summary>
		public Sprite Icon { get => icon; }

		/// <summary>
		/// The background for the module.
		/// </summary>
		public Sprite Background { get => background; }

		/// <summary>
		/// The description for the module.
		/// </summary>
		public string Description { get => description; }

		/// <summary>
		/// The size of this module.
		/// </summary>
		public ModuleSize ModuleSize { get => size; }

		/// <summary>
		/// Does this module add effects to the ship entity.
		/// </summary>
		public bool HasShipEffects { get => hasShipEffects; }

		/// <summary>
		/// The effects to add to the ship entity, if used.
		/// </summary>
		public EffectWrapper[] ShipEffects { get => shipEffects; }

		/// <summary>
		/// Does this module add effects to the ships weapon entities.
		/// </summary>
		public bool HasWeaponEffects { get => hasWeaponEffects; }

		/// <summary>
		/// The effects to add to the ships weapon entities, if used.
		/// </summary>
		public EffectWrapper[] WeaponEffects { get => weaponEffects; }

		/// <summary>
		/// Does this module add effects to the ships projectile entities.
		/// </summary>
		public bool HasProjectileEffects { get => hasProjectileEffects; }

		/// <summary>
		/// The effects to add to the ships projectile entities, if used.
		/// </summary>
		public EffectWrapper[] ProjectileEffects { get => projectileEffects; }

		public override string Tooltip => $"A <color=\"orange\">{size}</color> module.";

		protected virtual void OnValidate()
		{
			if (prefab != null && prefab.HasComponent<ModuleEntity>() == false)
			{
				Debug.LogError($"Assigned prefab must have a {nameof(ModuleEntity)} attatched to it.", this);
			}
		}

		private static bool OnLayoutDraw(Rect rect, bool value)
		{
			if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
			{
				value = !value;
				GUI.changed = true;
				Event.current.Use();
			}

#if UNITY_EDITOR
			UnityEditor.EditorGUI.DrawRect(rect.Padding(1), value ? new Color(1f, 1f, 1f, 1f) : new Color(0f, 0f, 0f, 0f));
#endif
			return value;
		}
	}
}
