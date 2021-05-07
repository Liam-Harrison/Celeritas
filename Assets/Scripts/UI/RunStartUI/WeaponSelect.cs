using Celeritas.Game;
using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : Singleton<WeaponSelect>
{
	private List<WeaponData> WeaponList;
	private int PseudoIterator;

	private void Awake()
	{
		EntityDataManager.OnLoadedAssets += () =>
		{
			WeaponList = new List<WeaponData>();
			foreach (WeaponData weapon in EntityDataManager.Instance.Weapons)
			{
				//Debug.Log("Weapon: " + weapon.Title);
				WeaponList.Add(weapon);
			}
			//WeaponList.Sort();
			//Debug.Log("WeaponList size " + WeaponList.Count);
			//TODO: read from playerdata, set last used weapon if avaliable
			PseudoIterator = 0;
			SetActiveWeapon();
		};
		base.Awake();
	}

	public TextMeshProUGUI WeaponNameTxt;
	public TextMeshProUGUI WeaponDescTxt;
	public Image Icon;
	[SerializeField] WeaponData currentWeapon;
	public WeaponData CurrentWeapon { get => currentWeapon; }
	public void SetActiveWeapon()
	{
		currentWeapon = WeaponList[PseudoIterator];
		WeaponNameTxt.SetText(currentWeapon.Title);
		WeaponDescTxt.SetText(currentWeapon.Description);
		Icon.sprite = currentWeapon.Icon;
		foreach (WeaponEntity ShipWeapon in ShipSelect.Instance.CurrentShip.Prefab.GetComponent<PlayerShipEntity>().WeaponEntities)
		{
			ShipWeapon.AttatchedModule.SetModule(currentWeapon);
		}
	}

	public void ScrollRight()
	{
		PseudoIterator++;
		if (PseudoIterator > WeaponList.Count - 1) //0-indexing
		{
			PseudoIterator = 0; //looping
		}
		SetActiveWeapon();
	}

	public void ScrollLeft()
	{
		PseudoIterator--;
		if (PseudoIterator < 0)
		{
			PseudoIterator = WeaponList.Count - 1;//0-indexing, looping
		}
		SetActiveWeapon();
	}
}
