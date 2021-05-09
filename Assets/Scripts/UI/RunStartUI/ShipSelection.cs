using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelection : MonoBehaviour
{
	private int ShipPseudoIterator;
	public GameObject ShipSpawn;
	private List<GameObject> ShipObjects;
	private GameObject TempShip;
	private List<WeaponData> WeaponList;
	private int WeaponPseudoIterator;

	private void Awake()
	{
		ShipObjects = new List<GameObject>();

		EntityDataManager.OnLoadedAssets += () =>
		{
			Destroy(ShipSpawn.GetComponentInChildren<ShipEntity>().gameObject); //removes placeholder ship, so we can loop without worrying about duplicates
			foreach (ShipData ship in EntityDataManager.Instance.PlayerShips)
			{
				TempShip = Instantiate(ship.Prefab, ShipSpawn.transform);
				Debug.Log(TempShip.name);
				ShipObjects.Add(TempShip);
			}
			ShipPseudoIterator = 0;
			//TODO: read from playerdata, set last used ship if avaliable (once we HAVE more than one ship...)

			WeaponList = new List<WeaponData>();
			foreach (WeaponData weapon in EntityDataManager.Instance.Weapons)
			{
				//Debug.Log("Weapon: " + weapon.Title);
				WeaponList.Add(weapon);
			}
			//WeaponList.Sort();
			//Debug.Log("WeaponList size " + WeaponList.Count);
			//TODO: read from playerdata, set last used weapon if avaliable
			WeaponPseudoIterator = 0;


			SetActiveShip();
			SetActiveWeapon();
		};

	}

	//////////////////
	///WEAPON STUFF///
	//////////////////

	public TextMeshProUGUI WeaponNameTxt;
	public TextMeshProUGUI WeaponDescTxt;
	public Image WeaponIcon;
	private WeaponData CurrentWeapon;
	public void SetActiveWeapon()
	{
		CurrentWeapon = WeaponList[WeaponPseudoIterator];
		WeaponNameTxt.SetText(CurrentWeapon.Title);
		WeaponDescTxt.SetText(CurrentWeapon.Description);
		WeaponIcon.sprite = CurrentWeapon.Icon;
		foreach (WeaponEntity ShipWeapon in CurrentShip.ShipData.Prefab.GetComponent<PlayerShipEntity>().WeaponEntities)
		{
			ShipWeapon.AttatchedModule.SetModule(CurrentWeapon);
		}
	}

	public void WeaponScrollRight()
	{
		WeaponPseudoIterator++;
		if (WeaponPseudoIterator > WeaponList.Count - 1) //0-indexing
		{
			WeaponPseudoIterator = 0; //looping
		}
		SetActiveWeapon();
	}

	public void WeaponScrollLeft()
	{
		WeaponPseudoIterator--;
		if (WeaponPseudoIterator < 0)
		{
			WeaponPseudoIterator = WeaponList.Count - 1;//0-indexing, looping
		}
		SetActiveWeapon();
	}

	  ////////////////
	 ///SHIP STUFF///
	////////////////

	public TextMeshProUGUI ShipClassTxt;
	public TextMeshProUGUI ShipRankTxt;
	public TextMeshProUGUI ShipNameTxt;
	public TextMeshProUGUI ShipDescTxt;
	private ShipEntity CurrentShip;
	private void SetActiveShip()
	{
		CurrentShip = ShipObjects[ShipPseudoIterator].GetComponent<ShipEntity>();
		Debug.Log(CurrentShip.name);
		Debug.Log(CurrentShip.ShipData.name);
		ShipRankTxt.SetText(CurrentShip.ShipData.Title);
		//ShipClassTxt.SetText(currentShip.ShipClass.ToString());
		//if (currentShip.ShipClass == ShipClass.Corvette)
		//{
		//	ShipClassTxt.SetText("Corvette");
		//}
		//else if (currentShip.ShipClass == ShipClass.Battleship)
		//{
		//	ShipClassTxt.SetText("Battleship");
		//}
		//else if (currentShip.ShipClass == ShipClass.Destroyer)
		//{
		//	ShipClassTxt.SetText("Destroyer");
		//}
		//else
		//{
		//	ShipClassTxt.SetText("Class Not Set");
		//}

		ShipNameTxt.SetText("Celerity [NaN]"); //TODO: add run number once saving implemented
											   //ShipDescTxt.SetText(CurrentShip.Description);
		ShipDescTxt.SetText("TODO: add ship descriptions");

		//set current ship prefab to active, all others inactive
		foreach (GameObject ship in ShipObjects)
		{
			ship.SetActive(ship == CurrentShip);
		}
		SetActiveWeapon(); //update the ship so it has the correct weapon

	}

	//TODO: add more complicated logic for ship categorisation/scrolling
	// when we actually have multiple ships.

	public void ShipScrollRight()
	{
		ShipPseudoIterator++;
		if (ShipPseudoIterator > ShipObjects.Count - 1) //0-indexing
		{
			ShipPseudoIterator = 0; //looping
		}
		SetActiveShip();
	}

	public void ShipScrollLeft()
	{
		ShipPseudoIterator--;
		if (ShipPseudoIterator < 0)
		{
			ShipPseudoIterator = ShipObjects.Count - 1;//0-indexing, looping
		}
		SetActiveShip();
	}
}
