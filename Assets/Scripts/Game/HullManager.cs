using System.Collections.Generic;
using Celeritas.Scriptables;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Celeritas.Extensions;

/// <summary>
/// Uses HullData to generate and manage an in-game hull
/// </summary>
[InlineEditor]
public class HullManager : MonoBehaviour
{
	// Inspector
	[SerializeField]
	private GameObject iconPlane;

	[SerializeField]
	[OnValueChanged(nameof(onAnyDataChanged))]
	private GameObject HullWall;

	[SerializeField]
	[OnValueChanged(nameof(onAnyDataChanged))]
	private GameObject HullFloor;

	[Space]
	[AssetList]
	[SerializeField]
	[OnValueChanged(nameof(onAnyDataChanged))]
	private HullData hullData;

	// Internal
	private GameObject hullGroup;
	private GameObject moduleGroup;
	private GameObject wallGroup;
	private GameObject floorGroup;

	private enum Direction
	{
		North,
		East,
		South,
		West
	}
	
	private Dictionary<Direction, Vector2> DirectionMap = new Dictionary<Direction, Vector2> {
			{Direction.North, new Vector2(0,1) },
			{Direction.East, new Vector2(1,0) },
			{Direction.South, new Vector2(0,-1) },
			{Direction.West, new Vector2(-1,0) },
		};

	// Properties

	/// <summary>
	/// Grabs the ship's hull data scriptable object.
	/// </summary>
	public HullData HullData { get => hullData; }

	private void Start()
	{	
		GenerateAll();
		if (Application.isPlaying) HideHullIfNotInBuildMode();
	}

	private void OnEnable()
	{
		StateManager.onStateChanged += HideHullIfNotInBuildMode;
	}

	private void OnDisable()
	{
		StateManager.onStateChanged -= HideHullIfNotInBuildMode;
	}

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

		hullData.HullLayout.ForEach((x, y) =>
		{
			if (hullData.HullLayout[x, y] == true)
			{
				foreach (KeyValuePair<Direction, Vector2> pair in DirectionMap)
				{
					int newX = x + (int)pair.Value.x;
					int newY = y + (int)pair.Value.y;
					// Check if edge (No tiles)
					if (newX >= 0 && newX < hullData.HullLayout.GetLength(0) && newY >= 0 && newY < hullData.HullLayout.GetLength(1))
					{
						bool cell = hullData.HullLayout[newX, newY];
						if (cell == false) // If there is space
						{
							PlaceWalls(x, y, pair.Key, wallGroup);
						}
					}
					else
					{
						PlaceWalls(x, y, pair.Key, wallGroup);
					}
				}
			}
		});

		wallGroup.transform.rotation = transform.rotation;
	}

	/// <summary>
	/// Generates ship hull floors.
	/// </summary>
	[ButtonGroup]
	private void GenerateFloor()
	{
		SetupMasterGroup();
		if (floorGroup != null) DestroyImmediate(floorGroup.gameObject);
		floorGroup = new GameObject(nameof(floorGroup));
		floorGroup.transform.position = transform.position;
		floorGroup.transform.parent = hullGroup.transform;

		hullData.HullLayout.ForEach((x, y) =>
		{
			if (hullData.HullLayout[x, y] == true)
			{
				PlaceFloor(x, y, floorGroup);
			}
		});

		floorGroup.transform.rotation = transform.rotation;
	}

	/// <summary>
	/// Generates ship hull module rooms.
	/// </summary>
	[ButtonGroup]
	private void GenerateModules()
	{
		SetupMasterGroup();
		if (moduleGroup != null) DestroyImmediate(moduleGroup.gameObject);
		moduleGroup = new GameObject(nameof(moduleGroup));
		moduleGroup.transform.position = transform.position;
		moduleGroup.transform.parent = hullGroup.transform;

		hullData.HullLayout.ForEach((x, y) =>
		{
			if (hullData.HullLayout[x, y] == true)
			{
				PlaceModules(x, y, moduleGroup);

			}
		});

		moduleGroup.transform.rotation = transform.rotation;
	}

	/// <summary>
	/// Generates ship hull floor, walls and modules.
	/// </summary>
	[Button("Generate All", ButtonSizes.Large)]
	public void GenerateAll()
	{
		GenerateFloor();
		GenerateWalls();
		GenerateModules();
	}

	[Button]
	private void ClearAll() {
		if (hullGroup != null) DestroyImmediate(hullGroup.gameObject);
	}

	private void PlaceWalls(int x, int y, Direction dir, GameObject group)
	{
		int centerY = y - hullData.HullLayout.GetLength(1) / 2;
		var coords = transform.position + new Vector3(x, 0, centerY);
		var wall = Instantiate(HullWall, coords, Quaternion.identity);
		wall.transform.parent = group.gameObject.transform;
		// Wall prefabs should start facing the east direction so no condition is required.
		switch (dir)
		{
			case Direction.North:
				{
					wall.name = "North";
					wall.transform.Rotate(transform.rotation.x, transform.rotation.y + -90, transform.rotation.z);
					break;
				}
			case Direction.South:
				{
					wall.name = "South";
					wall.transform.Rotate(0, 90, 0);
					break;
				}
			case Direction.West:
				{
					wall.name = "West";
					wall.transform.Rotate(0, 180, 0);
					break;
				}
		}
	}

	private void PlaceFloor(int x, int y, GameObject group)
	{
		int centerY = y - hullData.HullLayout.GetLength(1) / 2;
		var coords = transform.position + new Vector3(x, 0, centerY);
		var wall = Instantiate(HullFloor, coords, Quaternion.identity);
		GameObject locationGroup = new GameObject($"[{x},{y}]");
		wall.transform.parent = locationGroup.transform;
		locationGroup.transform.parent = group.transform;
	}

	private void PlaceModules(int x, int y, GameObject group)
	{
		var moduleData = hullData.HullModules[x, y];
		if (moduleData != null && moduleData.HullRoom != null)
		{
			int centerY = y - hullData.HullModules.GetLength(1) / 2;
			var coords = transform.position + new Vector3(x, 0, centerY);
			var module = Instantiate(moduleData.HullRoom, coords, Quaternion.identity);
			var icon = Instantiate(iconPlane, coords + new Vector3(0, 1, 0), Quaternion.identity);
			icon.transform.parent = module.transform;
			icon.GetComponent<SpriteRenderer>().sprite = moduleData.icon;
			GameObject locationGroup = new GameObject($"[{x},{y}]");
			module.transform.parent = locationGroup.transform;
			locationGroup.transform.parent = group.transform;
		}
	}

	private void HideHullIfNotInBuildMode()
	{
		hullGroup.SetActive(StateManager.IsInState(StateManager.States.BUILD) ? true : false);
	}

	private void onAnyDataChanged()
	{
		GenerateAll();
	}
}

