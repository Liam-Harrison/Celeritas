using AssetIcons;
using Celeritas.Game.Entities;
using Celeritas.Game.Events;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Contains the instanced information for a module.
	/// </summary>
	[CreateAssetMenu(fileName = "New Module", menuName = "Celeritas/New Module", order = 20)]
	public class ModuleData : EntityData
	{
		[SerializeField, TitleGroup("Module")]
		private ModuleSize size;

		[SerializeField]
		private ModuleCatagory catagory;

		[SerializeField]
		private Rarity rarity;

		[HorizontalGroup("Module Info", Width = 50), BoxGroup("Module Info/Icon"), SerializeField, PreviewField, HideLabel]
		[AssetIcon(maxSize: 50)]
		private Sprite icon;

		[HorizontalGroup("Module Info")]
		[BoxGroup("Module Info/Description")]
		[SerializeField, TextArea, HideLabel]
		private string description;

		[SerializeField, Title("Module Icon Cell"), FoldoutGroup("layout")]
		[TableMatrix(SquareCells = true, DrawElementMethod = nameof(OnIconLayoutDraw))]
		private bool[,] iconLayout = new bool[BaseLayoutResolution, BaseLayoutResolution];

		private static int BaseLayoutResolution = 3;

		public bool[,] IconLayout { get => iconLayout; }
		
		[SerializeField, Title("Module Layout")]
		private TetrisShape shape;
		

		/// <summary>
		/// The icon for the module.
		/// </summary>
		public Sprite Icon { get => icon; }

		/// <summary>
		/// The description for the module.
		/// </summary>
		public string Description { get => description; }

		/// <summary>
		/// The catagory of this module.
		/// </summary>
		public ModuleCatagory ModuleCatagory { get => catagory; }

		/// <summary>
		/// The size of this module.
		/// </summary>
		public ModuleSize ModuleSize { get => size; }

		/// <summary>
		/// The tetris shape used by this data.
		/// </summary>
		public TetrisShape TetrisShape { get => shape; }

		/// <summary>
		/// The rarity of this module.
		/// </summary>
		public Rarity Rarity { get => rarity; }

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

		private bool OnIconLayoutDraw(Rect rect, bool value)
		{
			if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
			{
				iconLayout = new bool[BaseLayoutResolution,BaseLayoutResolution];
				value = !value;
				GUI.changed = true;
				Event.current.Use();
			}

#if UNITY_EDITOR
			UnityEditor.EditorGUI.DrawRect(rect.Padding(5), value ? new Color(0f, 0f, 1f, 1f) : new Color(0f, 0f, 0f, 0f));
#endif
			return value;
		}
	}
}
