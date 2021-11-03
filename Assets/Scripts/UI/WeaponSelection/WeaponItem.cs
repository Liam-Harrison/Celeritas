using System;
using Celeritas.Scriptables;
using Celeritas.UI.General;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Celeritas.UI.WeaponSelection
{
	public class WeaponItem : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
	{
		[SerializeField, TitleGroup("Assignmnets")]
		private IconUI icon;

		public WeaponData Weapon { get; private set; }

		[SerializeField]
		private EventReference onDragSound;

		[SerializeField]
		private EventReference onDropSound;

		Runstart.WeaponSelection WeaponSelection { get; set; }

		void Awake()
		{
			WeaponSelection = FindObjectOfType<Runstart.WeaponSelection>();
		}

		public void SetWeapon(WeaponData weapon)
		{
			Weapon = weapon;

			icon.SetItem(weapon);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (!onDragSound.IsNull) RuntimeManager.PlayOneShot(onDragSound);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (!onDropSound.IsNull) RuntimeManager.PlayOneShot(onDropSound);
		}

		public void OnDrag(PointerEventData eventData)
		{
			WeaponSelection.StartDrag(this);
		}
	}
}