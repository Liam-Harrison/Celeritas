using Celeritas.Game.Controllers;
using Celeritas.UI.Inventory;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Celeritas;
using Celeritas.Scriptables;

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

					if (hull.Modules[grid.x, grid.y] != null && hull.Modules[grid.x, grid.y].HasModuleAttatched == false)
					{
						placeObject.transform.position = hull.GetWorldPositionGrid(grid.x, grid.y);
						placeObject.transform.rotation = hull.transform.rotation;

						placeObject.SetActive(true);
						canPlace = true;
					}
					else
					{
						placeObject.SetActive(false);
						canPlace = false;
					}
				}
				else
				{
					placeObject.SetActive(false);
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
			foreach (var renderer in renderers) {
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
