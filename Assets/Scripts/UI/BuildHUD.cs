using Celeritas.Game.Controllers;
using Celeritas.UI.Inventory;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Celeritas.UI
{
	public class BuildHUD : MonoBehaviour
	{
		[SerializeField, Title("Assignments")]
		private InventoryUI inventory;

		[SerializeField]
		private Image drop;

		[SerializeField]
		private Material placingMaterial;

		private InventoryItemUI dragging;

		private GameObject placeObject;

		private new Camera camera;

		private (int x, int y) grid;
		private bool canPlace = false;
		private bool placing = false;

		private void Awake()
		{
			camera = Camera.main;
		}

		private void OnEnable()
		{
			dragging = null;
			placing = false;
			drop.gameObject.SetActive(false);
		}

		private void Update()
		{
			if (placing)
			{
				var mousepos = Mouse.current.position.ReadValue();
				drop.transform.position = mousepos;
				var ray = camera.ScreenPointToRay(mousepos);

				if (Physics.Raycast(ray, out var hit, 40f, LayerMask.GetMask("Hull")))
				{
					var hull = PlayerController.Instance.PlayerShipEntity.HullManager;
					grid = hull.GetGridFromWorld(hit.point);

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
			else if (canPlace)
			{
				canPlace = false;
				var ship = PlayerController.Instance.PlayerShipEntity;
				ship.HullManager.Modules[grid.x, grid.y].SetModule(dragging.Module);
			}
		}

		public void OnItemDragBegin(InventoryItemUI item)
		{
			placing = true;
			dragging = item;
			drop.sprite = item.Module.icon;
			drop.gameObject.SetActive(true);

			if (placeObject != null)
			{
				Destroy(placeObject);
			}

			placeObject = Instantiate(item.Module.Prefab);
			placeObject.GetComponentInChildren<MeshRenderer>().sharedMaterial = placingMaterial;
			placeObject.SetActive(false);
		}

		public void OnItemDragStopped(InventoryItemUI item)
		{
			placing = false;

			if (placeObject != null)
				placeObject.SetActive(false);

			drop.gameObject.SetActive(false);
		}
	}
}
