using System.Collections;
using System.Collections.Generic;
using Celeritas.Scriptables;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Celeritas.Game {

	public enum Direction
	{
		North,
		East,
		South,
		West
	}

	public class HullBuilder : MonoBehaviour
	{
		[OnValueChanged(nameof(onAnyDataChanged))]
		public GameObject HullWall;

		[OnValueChanged(nameof(onAnyDataChanged))]
		public GameObject HullFloor;

		[Space]
		[AssetList]
		[PreviewField(50)]
		[OnValueChanged(nameof(onAnyDataChanged))]
		public HullData Hull;
		public GameObject group;

		[Button("Re-Generate")]
		public void Generate()
		{
			if (group != null) DestroyImmediate(group.gameObject);
			group = new GameObject("Hull");
			group.transform.position = transform.position;
			group.transform.parent = transform;
			if (Application.isPlaying) HideHullIfNotInBuildMode();
			GenerateWalls();
		}


		Dictionary<Direction, Vector2> DirectionMap = new Dictionary<Direction, Vector2> {
			{Direction.North, new Vector2(0,1) },
			{Direction.East, new Vector2(1,0) },
			{Direction.South, new Vector2(0,-1) },
			{Direction.West, new Vector2(-1,0) },
		};

		public void PlaceWalls(int x, int y, Direction dir, GameObject group)
		{
			int center = y - (Hull.HullLayout.GetLength(1) - 1) / 2;
			var wall = Instantiate(HullWall, new Vector3(transform.position.x + x, transform.position.y,transform.position.z + center), Quaternion.identity);
			wall.transform.parent = group.gameObject.transform;
			switch (dir)
			{
				case Direction.North:
					{
						wall.name = "North";
						wall.transform.Rotate(transform.rotation.x,transform.rotation.y + -90,transform.rotation.z);
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

		public void PlaceFloor(int x, int y, GameObject group) {
			int center = y - Hull.HullLayout.GetLength(1) / 2;
			var wall = Instantiate(HullFloor, new Vector3(transform.position.x + x, transform.position.y,transform.position.z + center), Quaternion.identity);
			wall.transform.parent = group.gameObject.transform;
		}

		public void GenerateWalls()
		{
			if (Hull != null)
			{
				for (int x = 0; x < Hull.HullLayout.GetLength(0); x++)
				{
					for (int y = 0; y < Hull.HullLayout.GetLength(1); y++)
					{
						if (Hull.HullLayout[x, y])
						{
							PlaceFloor(x,y,group);
							foreach (KeyValuePair<Direction, Vector2> pair in DirectionMap)
							{
								int newX = x + (int)pair.Value.x;
								int newY = y + (int)pair.Value.y;
								// Check if edge
								if (newX >= 0 && newX < Hull.HullLayout.GetLength(0) && newY >= 0 && newY < Hull.HullLayout.GetLength(1))
								{
									bool currentCell = Hull.HullLayout[newX, newY];
									if (!currentCell)
									{ // If there is space
										PlaceWalls(x, y, pair.Key, group);
									}
								} else {
									PlaceWalls(x, y, pair.Key, group);
								}

							}
						}
					}
				}
			}
			group.transform.rotation = transform.rotation;
		}

		private void Start() {
			Generate();
		}

		private void OnEnable() {
			StateManager.OnStateChanged += HideHullIfNotInBuildMode;
		}

		private void OnDisable() {
			StateManager.OnStateChanged -= HideHullIfNotInBuildMode;
		}

		void HideHullIfNotInBuildMode() {
			group.SetActive(StateManager.IsInState(StateManager.States.BUILD) ? true : false);
		}

		public void onAnyDataChanged()
		{
			Generate();
		}
	}
}
