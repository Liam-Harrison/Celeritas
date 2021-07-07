using Celeritas.Scriptables;
using Celeritas.UI.General;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Celeritas.UI.WeaponSelection
{
	public class WeaponItem : MonoBehaviour, IDragHandler
	{
		[SerializeField, TitleGroup("Assignmnets")]
		private IconUI icon;

		public WeaponData Weapon { get; private set; }

		public void SetWeapon(WeaponData weapon)
		{
			Weapon = weapon;

			icon.SetItem(weapon);
		}
		public void OnDrag(PointerEventData eventData)
		{

		}
	}
}