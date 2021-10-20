using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Celeritas.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeritas.Game.Events
{
	[CreateAssetMenu(fileName = "New Event", menuName = "Celeritas/Events/New Event", order = 10)]
	public class EventData : SerializedScriptableObject
	{
		private const int MAX_CHUNK_SIZE = 5;

		private static readonly int MIDDLE = MAX_CHUNK_SIZE / 2;

		[SerializeField, TitleGroup("Settings", "The general settings for this event.")]
		private bool cannotUnloadDynamically = false;

		[SerializeField, TitleGroup("Settings")]
		private bool cannotAppearRandomly = false;

		[SerializeField, TitleGroup("Settings"), Indent(11), TableMatrix(DrawElementMethod = "DrawCell", HideColumnIndices = true, HideRowIndices = true, SquareCells = true, HorizontalTitle = "X Axis", VerticalTitle = "Y Axis")]
		private bool[,] grid;

		[SerializeField, TitleGroup("Settings")]
		private int spawnWeight = 100;

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

		[SerializeField, TitleGroup("Event")]
		private EventOutcome eventOutcome;

		public bool CannotUnloadDynamically { get => cannotUnloadDynamically; }

		public bool CannotAppearRandomly { get => cannotAppearRandomly; }

		public bool [,] Grid { get => grid; }

		public bool ShowOnMap { get => showOnMap; }

		public int SpawnWeight { get => spawnWeight;  }

		public bool ShowArrow { get => showArrow; }

		public Color MapColor { get => mapColor; }

		public Color ArrowColor { get => arrowColor; }

		public string ArrowText { get => arrowText; }

		public Sprite ArrowIcon { get => arrowIcon; }

		public EventOutcome EventOutcome {  get => eventOutcome; }

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
			if (y == MAX_CHUNK_SIZE)
			{
				y = 0;
				x++;
			}

			return value;
		}
#endif
	}
}
