using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
	private List<WeaponData> WeaponList;
	private int PseudoIterator;

	private void Awake()
	{
		WeaponList = new List<WeaponData>();
		foreach (WeaponData weapon in EntityDataManager.Instance.Weapons)
		{
			WeaponList.Add(weapon);
		}
		WeaponList.Sort();
		//TODO: read from playerdata, set last used weapon if avaliable
		PseudoIterator = 0;
		SetActiveWeapon();
	}

	public TextMeshProUGUI WeaponNameTxt;
	private void SetActiveWeapon()
	{
		WeaponNameTxt.SetText(WeaponList[PseudoIterator].name);
		//TODO: add other fields
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
