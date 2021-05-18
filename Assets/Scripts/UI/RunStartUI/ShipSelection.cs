using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelection : MonoBehaviour
{
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

	public static ShipEntity CurrentShip { get; private set; }

	private List<ShipEntity> shipObjects;
	private List<WeaponData> weaponList;

	private int shipPseudoIterator;
	private int weaponPseudoIterator;

	private void Awake()
	{
		shipObjects = new List<ShipEntity>();

		Debug.Log($"ships: {EntityDataManager.Instance.PlayerShips.Count}");
		foreach (ShipData data in EntityDataManager.Instance.PlayerShips)
		{
			var ship = EntityDataManager.InstantiateEntity<ShipEntity>(data);

			ship.IsStationary = true;
			ship.transform.parent = shipSpawn;
			ship.transform.localPosition = Vector3.zero;
			ship.transform.localRotation = Quaternion.identity;
			ship.transform.localScale = Vector3.one;
			ship.gameObject.SetActive(false);

			shipObjects.Add(ship);
		}

		weaponList = new List<WeaponData>();
		foreach (WeaponData weapon in EntityDataManager.Instance.Weapons)
		{
			weaponList.Add(weapon);
		}
	}

	private void OnEnable()
	{
		weaponPseudoIterator = 0;
		shipPseudoIterator = 0;
		SetActiveShip();
		SetActiveWeapon();
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

	public void WeaponScrollRight()
	{
		if (++weaponPseudoIterator > weaponList.Count - 1)
			weaponPseudoIterator = 0;

		SetActiveWeapon();
	}

	public void WeaponScrollLeft()
	{
		if (++weaponPseudoIterator < 0)
			weaponPseudoIterator = weaponList.Count - 1;

		SetActiveWeapon();
	}

	private void SetActiveShip()
	{
		CurrentShip = shipObjects[shipPseudoIterator].GetComponent<ShipEntity>();
		shipRankTxt.SetText(CurrentShip.ShipData.Title);
		shipClassTxt.SetText(CurrentShip.ShipData.ShipClass.ToString());
		shipNameTxt.SetText("Celerity <i>#1</i>");
		shipDescTxt.SetText(CurrentShip.ShipData.Description);
		weaponIcon.sprite = CurrentShip.ShipData.Icon;

		foreach (ShipEntity ship in shipObjects)
		{
			ship.gameObject.SetActive(ship == CurrentShip);
		}

		SetActiveWeapon();
	}

	public void ShipScrollRight()
	{
		if (++shipPseudoIterator > shipObjects.Count - 1)
			shipPseudoIterator = 0;

		SetActiveShip();
	}

	public void ShipScrollLeft()
	{
		if (--shipPseudoIterator < 0)
			shipPseudoIterator = shipObjects.Count - 1;

		SetActiveShip();
	}
}
