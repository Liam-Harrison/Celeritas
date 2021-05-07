using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelect : Singleton<ShipSelect>
{
	private int PseudoIterator;
	public GameObject ShipSpawn;
	private List<GameObject> ShipObjects;
	private GameObject TempShip;
	private void Awake()
	{
		Debug.Log("ShipSelect Awake");
		EntityDataManager.OnLoadedAssets += () =>
		{
			Debug.Log("ShipSelect Awake OnLoadedAssets");
			Destroy(ShipSpawn.GetComponentInChildren<ShipEntity>().gameObject); //removes placeholder ship, so we can loop without worrying about duplicates
			foreach (ShipData ship in EntityDataManager.Instance.PlayerShips)
			{
				TempShip = Instantiate(ship.Prefab, ShipSpawn.transform);
				ShipObjects.Add(TempShip);
			}
			PseudoIterator = 0;
			//TODO: read from playerdata, set last used ship if avaliable (once we HAVE more than one ship...)
			SetActiveShip();
		};
		base.Awake();
	}

	public TextMeshProUGUI ShipClassTxt;
	public TextMeshProUGUI ShipRankTxt;
	public TextMeshProUGUI ShipNameTxt;
	public TextMeshProUGUI ShipDescTxt;
	[SerializeField] private ShipData currentShip;
	public ShipData CurrentShip { get => currentShip; }
	private void SetActiveShip()
	{
		currentShip = ShipObjects[PseudoIterator].GetComponent<ShipData>();
		ShipRankTxt.SetText(currentShip.Title);
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
			ship.SetActive(ship == ShipObjects[PseudoIterator]);
		}
		WeaponSelect.Instance.SetActiveWeapon(); //update the ship so it has the correct weapon

	}

	//TODO: add more complicated logic for ship categorisation/scrolling
	// when we actually have multiple ships.

	public void ScrollRight()
	{
		PseudoIterator++;
		if (PseudoIterator > ShipObjects.Count - 1) //0-indexing
		{
			PseudoIterator = 0; //looping
		}
		SetActiveShip();
	}

	public void ScrollLeft()
	{
		PseudoIterator--;
		if (PseudoIterator < 0)
		{
			PseudoIterator = ShipObjects.Count - 1;//0-indexing, looping
		}
		SetActiveShip();
	}

}
