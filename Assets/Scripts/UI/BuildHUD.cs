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

		private GameObject placing;

		private new Camera camera;

		private void Awake()
		{
			camera = Camera.main;
		}

		private void OnEnable()
		{
			dragging = null;
			drop.gameObject.SetActive(false);
		}

		private void Update()
		{
			if (dragging != null)
			{
				var mousepos = Mouse.current.position.ReadValue();
				drop.transform.position = mousepos;
				var ray = camera.ScreenPointToRay(mousepos);

				if (Physics.Raycast(ray, out var hit, 40f, LayerMask.GetMask("Hull")))
				{
					Debug.Log($"{PlayerController.Instance.PlayerShipEntity.HullManager.GetGridFromWorld(hit.point)}", hit.collider);
				}
			}
		}

		public void OnItemDragBegin(InventoryItemUI item)
		{
			dragging = item;
			drop.sprite = item.Module.icon;
			drop.gameObject.SetActive(true);

			if (placing != null)
			{
				Destroy(placing);
			}

			placing = Instantiate(item.Module.Prefab);
			placing.GetComponent<MeshRenderer>().sharedMaterial = placingMaterial;
			placing.SetActive(false);
		}

		public void OnItemDragStopped(InventoryItemUI item)
		{
			dragging = null;
			drop.gameObject.SetActive(false);
		}
	}
}
