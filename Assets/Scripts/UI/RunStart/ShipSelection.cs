using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Celeritas.UI.General;
using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeritas.UI.Runstart
{
	public class ShipSelection : MonoBehaviour
	{
		[SerializeField, Title("Assignments")]
		private new CinemachineVirtualCamera camera;

		[SerializeField, Title("Spawn")]
		private Transform shipSpawn;

		public static PlayerShipEntity CurrentShip { get; private set; }

		private readonly Dictionary<ShipData, PlayerShipEntity> shipObjects = new Dictionary<ShipData, PlayerShipEntity>();
		private readonly List<WeaponData> weaponList = new List<WeaponData>();

		private WeaponData currentWeapon;

		private void Awake()
		{
			if (EntityDataManager.Instance != null && EntityDataManager.Instance.Loaded)
				SetupData();
			else
				EntityDataManager.OnLoadedAssets += SetupData;
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
	}
}