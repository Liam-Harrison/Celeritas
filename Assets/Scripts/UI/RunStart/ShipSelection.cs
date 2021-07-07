using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.UI.Runstart
{
	public class ShipSelection : MonoBehaviour
	{
		[SerializeField]
		private Vector3 rotation;

		[SerializeField, Title("Assignments")]
		private new CinemachineVirtualCamera camera;

		[SerializeField, Title("Spawn")]
		private Transform shipSpawn;

		/// <summary>
		/// The currently selected ship.
		/// </summary>
		public PlayerShipEntity CurrentShip { get; private set; }

		/// <summary>
		/// The spawn transform of the ship.
		/// </summary>
		public Transform ShipSpawn { get => shipSpawn; }

		private readonly Dictionary<ShipData, PlayerShipEntity> shipObjects = new Dictionary<ShipData, PlayerShipEntity>();
		private readonly List<WeaponData> weaponList = new List<WeaponData>();

		private void Awake()
		{
			if (EntityDataManager.Instance != null && EntityDataManager.Instance.Loaded)
				SetupData();
			else
				EntityDataManager.OnLoadedAssets += SetupData;
		}

		/// <summary>
		/// Select a specific player ship.
		/// </summary>
		/// <param name="ship">The player ship data to select.</param>
		public void SelectShip(ShipData ship)
		{
			if (shipObjects.Count == 0)
				SetupData();

			if (!shipObjects.ContainsKey(ship))
				return;

			if (CurrentShip != null)
				CurrentShip.gameObject.SetActive(false);

			CurrentShip = shipObjects[ship];
			camera.m_Lens.OrthographicSize = CurrentShip.SelectionViewSize;

			CurrentShip.gameObject.SetActive(true);
		}

		/// <summary>
		/// Rotate the ship spawn to the provided rotation.
		/// </summary>
		/// <param name="rotation">The rotation to move to.</param>
		public void RotateOrigin(Vector3 rotation)
		{
			StopAllCoroutines();
			StartCoroutine(RotateCoroutine(rotation));
		}

		private IEnumerator RotateCoroutine(Vector3 rotation)
		{
			float start = Time.time;
			float time = 1;
			float p;

			do
			{
				p = Mathf.Clamp01((Time.time - start) / time);
				ShipSpawn.rotation = Quaternion.Slerp(ShipSpawn.rotation, Quaternion.Euler(rotation), Mathf.Sin(p));
				yield return null;
			} while (p < 1);

			ShipSpawn.rotation = Quaternion.Euler(rotation);

			yield break;
		}

		private void SetupData()
		{
			EntityDataManager.OnLoadedAssets -= SetupData;

			if (shipObjects.Count > 0)
				return;

			foreach (ShipData data in EntityDataManager.Instance.PlayerShips)
			{
				var ship = EntityDataManager.InstantiateEntity<PlayerShipEntity>(data);

				ship.IsStationary = true;
				ship.transform.parent = shipSpawn;
				ship.transform.localRotation = Quaternion.identity;
				ship.gameObject.SetActive(false);

				shipObjects.Add(data, ship);
			}

			foreach (WeaponData weapon in EntityDataManager.Instance.Weapons)
			{
				weaponList.Add(weapon);
			}
		}
	}
}