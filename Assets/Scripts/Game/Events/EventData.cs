using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeritas.Game.Events
{
	public abstract class EventData : SerializedScriptableObject
	{
		private const int MAX_CHUNK_SIZE = 6;

		private static readonly int MIDDLE = MAX_CHUNK_SIZE / 2 - 1;

		[SerializeField, TitleGroup("Settings", "The general settings for this event.")]
		private Sprite icon;

		[SerializeField, Indent(11), TitleGroup("Settings"), TableMatrix(DrawElementMethod = "DrawCell", HideColumnIndices = true, HideRowIndices = true, SquareCells = true, HorizontalTitle = "X Axis", VerticalTitle = "Y Axis")]
		private bool[,] grid;

		[SerializeField, TitleGroup("Map", "Information about the minimap and presentation.")]
		private bool showOnMap;

		[SerializeField, TitleGroup("Map"), ShowIf(nameof(showOnMap))]
		private Color mapColor = Color.white;

		[SerializeField, TitleGroup("Arrow")]
		private bool showArrow;

		[SerializeField, TitleGroup("Arrow"), ShowIf(nameof(showArrow))]
		private Color arrowColor = Color.white;

		[SerializeField, TitleGroup("Arrow", "ArrowText and ArrowIcon are optional. Only use if desired."), ShowIf(nameof(showArrow))]
		private string arrowText;

		[SerializeField, TitleGroup("Arrow"), ShowIf(nameof(showArrow))]
		private Sprite arrowIcon;

		public abstract void OnCreated();

		public abstract void OnEntered();

		public abstract void OnEnded();

		public abstract void OnUnloaded();

		public Sprite Icon { get => icon; }

		public bool [,] Grid { get => grid; }

		public bool ShowOnMap { get => showOnMap; }

		public bool ShowArrow { get => showArrow; }

		public Color MapColor { get => mapColor; }

		public Color ArrowColor { get => arrowColor; }

		public string ArrowText { get => arrowText; }

		public Sprite ArrowIcon { get => arrowIcon; }

		public Vector2Int GetRelativeToMiddle(int x, int y)
		{
			return new Vector2Int(x - MIDDLE, y - MIDDLE);
		}

#if UNITY_EDITOR
		[OnInspectorInit]
		private void SetupData()
		{
			if (grid == null)
			{
				grid = new bool[MAX_CHUNK_SIZE, MAX_CHUNK_SIZE];
				grid[MIDDLE, MIDDLE] = true;
			}
		}

		static int x = 0, y = 0;
		[OnInspectorGUI]
		private void OnEditorGui()
		{
			x = 0;
			y = 0;
		}

		private static bool DrawCell(Rect rect, bool value)
		{
			if (Event.current.type == EventType.MouseDown &&
				rect.Contains(Event.current.mousePosition) &&
				(x != MIDDLE || y != MIDDLE))
			{
				value = !value;
				GUI.changed = true;
				Event.current.Use();
			}

			Color color = value ? Color.white : new Color(0, 0, 0, 0.5f);
			if (x == MIDDLE && y == MIDDLE) color = Color.green;

			EditorGUI.DrawRect(rect.Padding(1), color);

			var style = new GUIStyle();
			style.normal.textColor = value ? Color.black : Color.white;
			var top = rect.HorizontalPadding(3).VerticalPadding(1).AlignTop(16);
			EditorGUI.LabelField(top, $"{x}, {y}", style);

			if (x == MIDDLE && y == MIDDLE)
				EditorGUI.LabelField(top.AlignBottom(0), $"Origin", style);

			y++;
			if (y == MAX_CHUNK_SIZE - 1)
			{
				y = 0;
				x++;
			}

			return value;
		}
#endif
	}
}
