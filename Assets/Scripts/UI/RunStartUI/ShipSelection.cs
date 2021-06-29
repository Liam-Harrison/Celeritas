using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

namespace Celeritas.UI
{
	public class ShipSelection : MonoBehaviour
	{
		[SerializeField, Title("Assignments")]
		private new CinemachineVirtualCamera camera;

		[SerializeField, Title("Spawn")]
		private Transform shipSpawn;

		[SerializeField, Title("Weapon UI")]
		private TextMeshProUGUI weaponNameTxt;

		[SerializeField]
		private TextMeshProUGUI weaponDescTxt;

		[SerializeField]
		private Image weaponIcon;

		[SerializeField, Title("Ship UI")]
		private TextMeshProUGUI shipClassTxt;

		[SerializeField]
		private TextMeshProUGUI shipRankTxt;

		[SerializeField]
		private TextMeshProUGUI shipNameTxt;

		[SerializeField]
		private TextMeshProUGUI shipDescTxt;

		[SerializeField]
		private Image shipIcon;

		public static PlayerShipEntity CurrentShip { get; private set; }

		private readonly List<PlayerShipEntity> shipObjects = new List<PlayerShipEntity>();
		private readonly List<WeaponData> weaponList = new List<WeaponData>();

		private int shipPseudoIterator;
		private int weaponPseudoIterator;

		private void Start()
		{
			if (EntityDataManager.Instance != null && EntityDataManager.Instance.Loaded)
				SetupData();
			else
				EntityDataManager.OnLoadedAssets += SetupData;
		}

		private void SetupData()
		{
			EntityDataManager.OnLoadedAssets -= SetupData;

			foreach (ShipData data in EntityDataManager.Instance.PlayerShips)
			{
				var ship = EntityDataManager.InstantiateEntity<PlayerShipEntity>(data);

				ship.IsStationary = true;
				ship.transform.parent = shipSpawn;
				ship.transform.localPosition = Vector3.zero;
				ship.transform.localRotation = Quaternion.identity;
				ship.transform.localScale = Vector3.one;
				ship.gameObject.SetActive(false);

				shipObjects.Add(ship);
			}

			foreach (WeaponData weapon in EntityDataManager.Instance.Weapons)
			{
				weaponList.Add(weapon);
			}

			weaponPseudoIterator = 0;
			shipPseudoIterator = 0;
			SetActiveShip();
			SetActiveWeapon();
		}

		/// <summary>
		/// Scroll the weapon selection one index up.
		/// </summary>
		public void WeaponScrollRight()
		{
			if (++weaponPseudoIterator > weaponList.Count - 1)
				weaponPseudoIterator = 0;

			SetActiveWeapon();
		}

		/// <summary>
		/// Scroll the weapon selection one index back.
		/// </summary>
		public void WeaponScrollLeft()
		{
			if (++weaponPseudoIterator < 0)
				weaponPseudoIterator = weaponList.Count - 1;

			SetActiveWeapon();
		}

		/// <summary>
		/// Scroll the ship selection one index up.
		/// </summary>
		public void ShipScrollRight()
		{
			if (++shipPseudoIterator > shipObjects.Count - 1)
				shipPseudoIterator = 0;

			SetActiveShip();
		}

		/// <summary>
		/// Scroll the ship selection one index back.
		/// </summary>
		public void ShipScrollLeft()
		{
			if (--shipPseudoIterator < 0)
				shipPseudoIterator = shipObjects.Count - 1;

			SetActiveShip();
		}

		private WeaponData currentWeapon;

		private void SetActiveWeapon()
		{
			currentWeapon = weaponList[weaponPseudoIterator];
			weaponNameTxt.SetText(currentWeapon.Title);
			weaponDescTxt.SetText(currentWeapon.Description);
			weaponIcon.sprite = currentWeapon.Icon;

			foreach (WeaponEntity ShipWeapon in CurrentShip.WeaponEntities)
			{
				ShipWeapon.AttatchedModule.SetModule(currentWeapon);
			}
		}

		private void SetActiveShip()
		{
			CurrentShip = shipObjects[shipPseudoIterator].GetComponent<PlayerShipEntity>();
			shipRankTxt.SetText(CurrentShip.ShipData.Title);
			shipClassTxt.SetText(CurrentShip.ShipData.ShipClass.ToString());
			shipNameTxt.SetText("Celerity <i>#1</i>");
			shipDescTxt.SetText(CurrentShip.ShipData.Description);
			weaponIcon.sprite = CurrentShip.ShipData.Icon;

			camera.m_Lens.OrthographicSize = CurrentShip.SelectionViewSize;

			foreach (ShipEntity ship in shipObjects)
			{
				ship.gameObject.SetActive(ship == CurrentShip);
			}

			SetActiveWeapon();
		}
	}
}