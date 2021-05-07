using System.Collections;
using System.Collections.Generic;
using Celeritas.Extensions;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class ModuleBuilder : SerializedMonoBehaviour
{
	private GameObject wallGroup;
	private GameObject hullGroup;

	[SerializeField]
	private GameObject hullWall;

	[SerializeField, Title("Module Layout")]
	[TableMatrix(SquareCells = true, DrawElementMethod = nameof(onLayoutDraw))]
	private bool[,] moduleLayout = new bool[BaseLayoutResolution, BaseLayoutResolution];

	private enum Direction
	{
		North,
		East,
		South,
		West
	}

	private Dictionary<Direction, Vector2> DirectionMap = new Dictionary<Direction, Vector2>
	{
		{Direction.North, new Vector2(0,1) },
		{Direction.East, new Vector2(1,0) },
		{Direction.South, new Vector2(0,-1) },
		{Direction.West, new Vector2(-1,0) },
	};

	private static int BaseLayoutResolution = 3;

	private void SetupMasterGroup()
	{
		if (hullGroup == null)
		{
			hullGroup = new GameObject(nameof(hullGroup));
			hullGroup.transform.position = transform.position;
			hullGroup.transform.parent = transform;
		}
	}

	/// <summary>
	/// Generates ship hull walls.
	/// </summary>
	[ButtonGroup]
	public void GenerateWalls()
	{
		SetupMasterGroup();
		if (wallGroup != null) DestroyImmediate(wallGroup.gameObject);
		wallGroup = new GameObject(nameof(wallGroup));
		wallGroup.transform.position = transform.position;
		wallGroup.transform.parent = hullGroup.transform;

		moduleLayout.ForEach((x, y) =>
		{
			if (moduleLayout[x, y] == true)
			{
				foreach (KeyValuePair<Direction, Vector2> pair in DirectionMap)
				{
					int newX = x + (int)pair.Value.x;
					int newY = y + (int)pair.Value.y;
					// Check if edge (No tiles)
					if (newX >= 0 && newX < moduleLayout.GetLength(0) && newY >= 0 && newY < moduleLayout.GetLength(1))
					{
						bool cell = moduleLayout[newX, newY];
						if (cell == false) // If there is space
						{
							PlaceWall(x, y, pair.Key, wallGroup);
						}
					}
					else
					{
						PlaceWall(x, y, pair.Key, wallGroup);
					}
				}
			}
		});
	}

	public Vector3 GetWorldPositionGrid(int x, int y)
	{
		int centerY = y - moduleLayout.GetLength(1) / 2;
		return transform.TransformPoint(new Vector3(centerY, x, 0));
	}


	private void PlaceWall(int x, int y, Direction dir, GameObject group)
	{
		var coords = GetWorldPositionGrid(x, y);
		var wall = Instantiate(hullWall, coords, Quaternion.identity);

		wall.transform.parent = group.gameObject.transform;
		wall.transform.localRotation = Quaternion.Euler(0, 90, -90);

		switch (dir)
		{
			case Direction.North:
				wall.name = "North";
				wall.transform.Rotate(0, -90, 0, Space.Self);
				break;

			case Direction.South:
				wall.name = "South";
				wall.transform.Rotate(0, 90, 0, Space.Self);
				break;

			case Direction.West:
				wall.name = "West";
				break;

			case Direction.East:
				wall.name = "East";
				wall.transform.Rotate(0, 180, 0, Space.Self);
				break;
		}
	}

	private static bool onLayoutDraw(Rect rect, bool value)
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
