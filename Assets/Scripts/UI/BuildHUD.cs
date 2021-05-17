using Celeritas.Game.Controllers;
using Celeritas.UI.Inventory;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Celeritas;
using Celeritas.Scriptables;
using Celeritas.Extensions;

namespace Celeritas.UI
{
	public class BuildHUD : MonoBehaviour
	{
		private InputActions.BasicActions actions = default;

		[SerializeField, Title("Assignments")]
		private InventoryUI inventory;

		[SerializeField]
		private Image drop;

		[SerializeField]
		private Material placingMaterial;

		private ModuleData dragging;

		private GameObject placeObject;

		private Camera mainCamera;

		private (int x, int y) grid;
		private bool canPlace = false;

		private void Awake()
		{
			mainCamera = Camera.main;
			actions = new InputActions.BasicActions(new InputActions());
		}

		private void OnEnable()
		{
			dragging = null;
			drop.gameObject.SetActive(false);

			actions.Enable();
			actions.Fire.performed += FirePerformed;
			actions.Fire.canceled += FireCanceled;
		}

		private void OnDisable()
		{
			actions.Disable();
			actions.Fire.performed -= FirePerformed;
			actions.Fire.canceled -= FireCanceled;
		}

		private void Update()
		{
			if (dragging != null)
			{
				var mousepos = Mouse.current.position.ReadValue();
				drop.transform.position = mousepos;
				var ray = mainCamera.ScreenPointToRay(mousepos);

				if (Physics.Raycast(ray, out var hit, 40f, LayerMask.GetMask("Hull")))
				{
					var hull = PlayerController.Instance.PlayerShipEntity.HullManager;
					grid = hull.GetGridFromWorld(hit.point);

					bool isOverModule = false; // Checks to see if any module is overlapping existing modules or overlapping the hull layout
					dragging.ModuleLayout.ForEach((x, y) =>
					{
						var currentModule = dragging.ModuleLayout[x, y];
						if (currentModule == true)
						{
							var newX = grid.x + x;
							var newY = grid.y + y;
							// Checks if hulllayout is out of bounds
							if (newX >= 0 && newX < hull.HullData.HullLayout.GetLength(0) && newY >= 0 && newY < hull.HullData.HullLayout.GetLength(1))
							{
								// checks if there an existing module overlapping or if its outside the ship hull
								if (hull.HullData.HullModules[newX, newY] != null || hull.HullData.HullLayout[newX, newY] == false)
								{
									isOverModule = true;
								}
							}
						}
					});


					if (hull.HullData.HullModules[grid.x, grid.y] == null && hull.Modules[grid.x, grid.y].HasModuleAttatched == false && isOverModule != true)
					{
						placeObject.SetActive(true);
						placingMaterial.color = new Color(0, 1, 0);
						canPlace = true;
					}
					else
					{
						placeObject.SetActive(true);
						placingMaterial.color = new Color(1, 0, 0);
						canPlace = false;
					}
					placeObject.transform.position = hull.GetWorldPositionGrid(grid.x, grid.y);
					placeObject.transform.rotation = hull.transform.rotation;
				}
				else
				{
					placeObject.SetActive(true);
					placingMaterial.color = new Color(1, 0, 0);
					canPlace = false;
				}
			}
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

			if (Physics.Raycast(ray, out var hit, 40f, LayerMask.GetMask("Hull")))
			{
				var hull = PlayerController.Instance.PlayerShipEntity.HullManager;
				grid = hull.GetGridFromWorld(hit.point);

				if (hull.Modules[grid.x, grid.y].HasModuleAttatched)
				{
					BeginDraggingModule(hull.Modules[grid.x, grid.y].AttatchedModule.ModuleData);
					hull.Modules[grid.x, grid.y].RemoveModule();
					hull.HullData.HullModuleOrigins[grid.x, grid.y] = null;
					hull.GenerateModuleWalls();
				}
			}
		}

		private void FireCanceled(InputAction.CallbackContext obj)
		{
			if (dragging != null && canPlace)
			{
				var ship = PlayerController.Instance.PlayerShipEntity;
				ship.HullManager.Modules[grid.x, grid.y].SetModule(dragging);
				ship.Inventory.Remove(dragging);
				ship.HullManager.HullData.HullModuleOrigins[grid.x, grid.y] = dragging;
				ship.HullManager.GenerateModuleWalls();

			}
			else if (dragging != null)
			{
				inventory.AddInventoryItem(dragging);

				var ship = PlayerController.Instance.PlayerShipEntity;
				ship.Inventory.Add(dragging);
			}

			canPlace = false;
			dragging = null;

			if (placeObject != null)
			{
				placeObject.SetActive(false);
				Destroy(placeObject);
			}

			drop.gameObject.SetActive(false);
		}

	}
}
