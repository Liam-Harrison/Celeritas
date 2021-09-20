using Celeritas.Game.Controllers;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.UI.Inventory;
using Celeritas.UI.Tooltips;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Celeritas.UI
{
	public class BuildHUD : MonoBehaviour
	{
		private InputActions.BasicActions actions = default;

		[SerializeField, Title("Assignments")]
		private InventoryUI inventory;

		[SerializeField]
		private ModuleSelectionUI moduleSelection;

		[SerializeField]
		private Image drop;

		[SerializeField]
		private Material placingMaterial;

		private ModuleData dragging;

		private GameObject placeObject;

		private Camera mainCamera;

		private ShowTooltip tooltip;

		private (int x, int y) grid;
		private bool canPlace = false, canUpgrade = false, replacing = false;
		private int replacingLevel;

		private void Awake()
		{
			mainCamera = Camera.main;
			actions = new InputActions.BasicActions(SettingsManager.InputActions);
			tooltip = GetComponent<ShowTooltip>();
		}

		private void OnEnable()
		{
			dragging = null;
			drop.gameObject.SetActive(false);

			actions.Enable();
			actions.Fire.performed += FirePerformed;
			actions.Fire.canceled += FireCanceled;

			if (LootController.Instance.ModuleComponents > 0)
			{
				ShowModuleDialog();
			}
		}

		private void OnDisable()
		{
			actions.Disable();
			actions.Fire.performed -= FirePerformed;
			actions.Fire.canceled -= FireCanceled;
		}

		private void Update()
		{
			var mousepos = Mouse.current.position.ReadValue();

			if (moduleSelection.gameObject.activeInHierarchy)
			{
				tooltip.HideTooltip();
				return;
			}

			if (dragging != null)
			{
				drop.transform.position = mousepos;
				RaycastModulePlacement(mousepos);
				tooltip.HideTooltip();
			}
			else
			{
				var ray = mainCamera.ScreenPointToRay(mousepos);

				if (Physics.Raycast(ray, out var hit, 40f, LayerMask.GetMask("Hull")))
				{
					var hull = PlayerController.Instance.PlayerShipEntity.HullManager;
					grid = hull.GetGridFromWorld(hit.point);

					if (hull.TryGetModuleEntity(grid.x, grid.y, out var entity))
					{
						tooltip.Show(entity);
					}
					else
					{
						tooltip.HideTooltip();
					}
				}
				else
				{
					tooltip.HideTooltip();
				}
			}
		}

		/// <summary>
		/// Internally update the placement of the dragged object.
		/// </summary>
		/// <param name="mousepos">The position of the mouse.</param>
		private void RaycastModulePlacement(Vector2 mousepos)
		{
			var ray = mainCamera.ScreenPointToRay(mousepos);

			if (Physics.Raycast(ray, out var hit, 40f, LayerMask.GetMask("Hull")))
			{
				var hull = PlayerController.Instance.PlayerShipEntity.HullManager;
				grid = hull.GetGridFromWorld(hit.point);

				bool isOverModule = false; // Checks to see if any module is overlapping existing modules or overlapping the hull layout
				dragging.TetrisShape.ModuleShape().ForEach((x, y) =>
				{
					var currentModule = dragging.TetrisShape.ModuleShape()[x, y];
					if (currentModule == true)
					{
						var newX = grid.x + x;
						var newY = grid.y + y;
						// Checks if hulllayout is out of bounds
						if (newX >= 0 && newX < hull.HullData.HullLayout.GetLength(0) && newY >= 0 && newY < hull.HullData.HullLayout.GetLength(1))
						{
							// checks if there an existing module overlapping or if its outside the ship hull
							if (hull.Entites[newX, newY] != null || hull.HullData.HullLayout[newX, newY] == false)
							{
								isOverModule = true;
							}
						}
					}
				});

				if (PlayerController.Instance.PlayerShipEntity.TryGetModuleEntity(dragging, out var entity))
				{
					placeObject.SetActive(true);
					placingMaterial.SetColor("Color_b8f2ba6eebb347aea8d6b1c5ee337cf4", new Color(0, 0, 1));

					placeObject.transform.position = entity.transform.position;
					placeObject.transform.rotation = hull.transform.rotation;

					canPlace = false;
					canUpgrade = true;
				}
				else
				{
					if (hull.Entites[grid.x, grid.y] == null && hull.Modules[grid.x, grid.y].HasModuleAttatched == false && isOverModule != true)
					{
						placeObject.SetActive(true);
						placingMaterial.SetColor("Color_b8f2ba6eebb347aea8d6b1c5ee337cf4", new Color(0, 1, 0));
						canPlace = true;
						canUpgrade = false;
					}
					else
					{
						placeObject.SetActive(true);
						placingMaterial.SetColor("Color_b8f2ba6eebb347aea8d6b1c5ee337cf4", new Color(1, 0, 0));
						canPlace = false;
						canUpgrade = false;
					}

					placeObject.transform.position = hull.GetWorldPositionGrid(grid.x, grid.y);
					placeObject.transform.rotation = hull.transform.rotation;
				}
			}
			else
			{
				placeObject.SetActive(false);
				placingMaterial.SetColor("Color_b8f2ba6eebb347aea8d6b1c5ee337cf4", new Color(1, 0, 0));
				canPlace = false;
				canUpgrade = false;
			}
		}

		private void ShowModuleDialog()
		{
			var rnd = new System.Random(System.DateTime.Now.Millisecond);

			var options = EntityDataManager.Instance.Modules
				.Select(v => new { v, i = rnd.Next() })
				.OrderBy(x => x.i).Take(3)
				.Select(x => x.v)
				.ToArray();

			moduleSelection.SelectOption(options, OnModuleSelected);
		}

		private void OnModuleSelected(ModuleData module)
		{
			LootController.Instance.GivePlayerLoot(LootType.Module, -1);

			PlayerController.Instance.PlayerShipEntity.Inventory.Add(module);
			inventory.AddInventoryItem(module);

			if (LootController.Instance.ModuleComponents > 0)
				ShowModuleDialog();
		}

		public void OnItemDragBegin(InventoryItemUI item)
		{
			BeginDraggingModule(item.Module);
			inventory.RemoveInventoryItem(item);
		}

		private void BeginDraggingModule(ModuleData module)
		{
			dragging = module;
			drop.sprite = module.Icon;
			drop.gameObject.SetActive(true);

			if (placeObject != null)
			{
				Destroy(placeObject);
			}

			placeObject = Instantiate(module.Prefab);
			var renderers = placeObject.GetComponentsInChildren<MeshRenderer>();
			foreach (var renderer in renderers)
			{
				renderer.sharedMaterial = placingMaterial;
			}
			placeObject.SetActive(false);
		}

		private void FirePerformed(InputAction.CallbackContext obj)
		{
			var mousepos = Mouse.current.position.ReadValue();
			var ray = mainCamera.ScreenPointToRay(mousepos);
			replacing = false;

			if (Physics.Raycast(ray, out var hit, 40f, LayerMask.GetMask("Hull")))
			{
				var hull = PlayerController.Instance.PlayerShipEntity.HullManager;
				grid = hull.GetGridFromWorld(hit.point);

				if (hull.TryGetModuleEntity(grid.x, grid.y, out var entity))
				{
					replacingLevel = entity.Level;
					replacing = true;

					BeginDraggingModule(entity.ModuleData);

					if (hull.TryGetModuleFromEntity(entity, out var module))
					{
						module.RemoveModule();
						hull.GenerateModuleWalls();
					}

					PlayerController.Instance.PlayerShipEntity.Inventory.Add(dragging);
				}
			}
		}

		private void FireCanceled(InputAction.CallbackContext obj)
		{
			if (dragging != null && canUpgrade)
			{
				var ship = PlayerController.Instance.PlayerShipEntity;
				if (ship.TryGetModuleEntity(dragging, out var entity))
				{
					entity.IncreaseLevel();
					ship.Inventory.Remove(dragging);
				}
			}
			else if (dragging != null && canPlace)
			{
				var ship = PlayerController.Instance.PlayerShipEntity;

				ship.HullManager.Modules[grid.x, grid.y].SetModule(dragging);
				ship.HullManager.GenerateModuleWalls();

				if (replacing)
				{
					ship.HullManager.Modules[grid.x, grid.y].AttatchedModule.SetLevel(replacingLevel);
				}

				ship.Inventory.Remove(dragging);
			}
			else if (dragging != null)
			{
				inventory.AddInventoryItem(dragging);

				if (replacing)
				{
					for (int i = 0; i < replacingLevel; i++)
					{
						inventory.AddInventoryItem(dragging);
					}
				}
			}

			canUpgrade = false;
			canPlace = false;
			dragging = null;
			replacing = false;

			if (placeObject != null)
			{
				placeObject.SetActive(false);
				Destroy(placeObject);
			}

			drop.gameObject.SetActive(false);
		}

	}
}
