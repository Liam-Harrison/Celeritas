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
	private List<ShipData> ShipList;
	private int PseudoIterator;

	private void Awake()
	{
		EntityDataManager.OnLoadedAssets += () =>
		{
			ShipList = new List<ShipData>();
			foreach (ShipData ship in EntityDataManager.Instance.PlayerShips)
			{
				ShipList.Add(ship);
			}
			//TODO: read from playerdata, set last used ship if avaliable (once we HAVE more than one ship...)
			PseudoIterator = 0;
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
		currentShip = ShipList[PseudoIterator];
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
		//TODO: add other fields
	}

	//TODO: add more complicated logic for ship categorisation/scrolling
	// when we actually have multiple ships.

	public void ScrollRight()
	{
		PseudoIterator++;
		if (PseudoIterator > ShipList.Count - 1) //0-indexing
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
			PseudoIterator = ShipList.Count - 1;//0-indexing, looping
		}
		SetActiveShip();
	}

}
