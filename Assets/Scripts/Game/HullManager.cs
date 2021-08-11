using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.Game
{
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
		private PlayerShipEntity playerShipEntity;

		[SerializeField]
		[OnValueChanged(nameof(onAnyDataChanged))]
		private GameObject hullWall;

		[SerializeField]
		[OnValueChanged(nameof(onAnyDataChanged))]
		private GameObject hullFloor;

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

		public Module[,] Modules { get; private set; }

		public ModuleEntity[,] Entites { get; private set; }

		public PlayerShipEntity PlayerShipEntity { get => playerShipEntity; }

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

		// Properties

		/// <summary>
		/// Grabs the ship's hull data scriptable object.
		/// </summary>
		public HullData HullData { get => hullData; }

		private void Start()
		{
			hullData.ResetModuleData();
			GenerateAll();
			hullGroup.SetActive(false);
		}

		private void OnEnable()
		{
			GameStateManager.onStateChanged += OnGameStateChanged;
		}

		private void OnDisable()
		{
			GameStateManager.onStateChanged -= OnGameStateChanged;
		}

		public Vector3 GetWorldPositionGrid(int x, int y)
		{
			int centerY = y - hullData.HullLayout.GetLength(1) / 2;
			return transform.TransformPoint(new Vector3(centerY, x, 0));
		}

		public (int x, int y) GetGridFromWorld(Vector3 pos)
		{
			var delta = transform.InverseTransformPoint(pos);
			var x = Mathf.RoundToInt(delta.y);
			var y = Mathf.RoundToInt(delta.x) + hullData.HullLayout.GetLength(1) / 2;
			return (x, y);
		}

		public bool TryGetModuleEntity(int x, int y, out ModuleEntity entity)
		{
			entity = Entites[x, y];
			return entity != null;
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

		/// <summary>
		/// Generates ship hull floors.
		/// </summary>
		[ButtonGroup]
		private void GenerateFloor()
		{
			SetupMasterGroup();

			if (floorGroup != null)
			{

				foreach (var module in Modules)
				{
					if (PlayerShipEntity.Modules.Contains(module))
						PlayerShipEntity.Modules.Remove(module);
				}

				Destroy(floorGroup.gameObject);
			}

			floorGroup = new GameObject(nameof(floorGroup));
			floorGroup.transform.position = transform.position;
			floorGroup.transform.parent = hullGroup.transform;

			Modules = new Module[hullData.HullLayout.GetLength(0), hullData.HullLayout.GetLength(1)];

			hullData.HullLayout.ForEach((x, y) =>
			{
				if (hullData.HullLayout[x, y] == true)
				{
					PlaceFloor(x, y, floorGroup);
				}
			});
		}

		/// <summary>
		/// Generates the walls for each module;
		/// </summary>
		[ButtonGroup]
		public void GenerateModuleWalls()
		{
			Entites = new ModuleEntity[hullData.HullLayout.GetLength(0), hullData.HullLayout.GetLength(1)];

			for (int x = 0; x < Modules.GetLength(0); x++)
			{
				for (int y = 0; y < Modules.GetLength(1); y++)
				{
					var module = Modules[x, y];

					if (module != null && module.HasModuleAttatched)
					{
						module.AttatchedModule.ModuleData.TetrisShape.ModuleShape().ForEach((mx, my) =>
						{
							if (module.AttatchedModule.ModuleData.TetrisShape.ModuleShape()[mx, my] == true)
							{
								Entites[x + mx, y + my] = module.AttatchedModule;
							}
						});
					}
				}
			}

			SetupMasterGroup();

			if (moduleGroup != null)
				Destroy(moduleGroup.gameObject);

			moduleGroup = new GameObject(nameof(moduleGroup));
			moduleGroup.transform.position = transform.position;
			moduleGroup.transform.parent = hullGroup.transform;

			moduleGroup.transform.rotation = FindObjectOfType<PlayerShipEntity>().gameObject.transform.rotation;

			for (int x = 0; x < hullData.HullLayout.GetLength(0); x++)
			{
				for (int y = 0; y < hullData.HullLayout.GetLength(1); y++)
				{
					var value = Entites[x, y];

					if (value != null)
					{
						foreach (KeyValuePair<Direction, Vector2> pair in DirectionMap)
						{
							int newX = x + (int)pair.Value.x;
							int newY = y + (int)pair.Value.y;
							if (newX >= 0 && newX < hullData.HullLayout.GetLength(0) && newY >= 0 && newY < hullData.HullLayout.GetLength(1))
							{
								var originalCell = Entites[x, y];
								var newCell = Entites[newX, newY];

								if (newCell != originalCell)
								{ // If cell does not match neighbour
									PlaceWall(x, y, pair.Key, moduleGroup);
								}
								else if (newCell == null) // If there is space
								{
									PlaceWall(x, y, pair.Key, moduleGroup);
								}

							}
						}
					}
				}
			}
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
		/// Generates ship hull floor, walls and modules.
		/// </summary>
		[Button("Generate All", ButtonSizes.Large)]
		public void GenerateAll()
		{
			GenerateFloor();
			GenerateWalls();
			GenerateModuleWalls();
		}

		/// <summary>
		/// Removes any generated mesh
		/// </summary>
		[Button]
		private void ClearAllMesh()
		{
			if (hullGroup != null) DestroyImmediate(hullGroup.gameObject);
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

		private void PlaceFloor(int x, int y, GameObject parent)
		{
			Vector3 coords = GetWorldPositionGrid(x, y);

			var floor = Instantiate(hullFloor, coords, Quaternion.identity);
			floor.name = $"[{x},{y}]";
			Modules[x, y] = floor.GetComponent<Module>();
			Modules[x, y].Initalize(PlayerShipEntity);
			PlayerShipEntity.Modules.Add(Modules[x, y]);

			floor.transform.parent = parent.transform;
		}

		private void OnDrawGizmos()
		{
			hullData.HullLayout.ForEach((x, y) =>
			{
				if (hullData.HullLayout[x, y] == true)
				{
					int centerY = y - hullData.HullLayout.GetLength(1) / 2;

					var coords = transform.position + new Vector3(centerY, x, 0);

					Gizmos.color = Color.green;
					Gizmos.DrawWireCube(coords, Vector3.one);
				}
			});
		}

		private void OnGameStateChanged(GameState state)
		{
			hullGroup.SetActive(state == GameState.BUILD);
		}

		private void onAnyDataChanged()
		{
			GenerateAll();
		}
	}
}