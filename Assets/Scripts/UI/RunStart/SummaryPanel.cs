using Celeritas.Game.Entities;
using Celeritas.UI.Runstart;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.WeaponSelection
{
	/// <summary>
	/// Logic for the 'summary panel' section of the ship selection / run start UI
	/// Right now it just ensures that the player ship has active weapons (no 'empty slots') before they can start their run.
	/// </summary>
	class SummaryPanel :MonoBehaviour
	{
		[SerializeField, Title("LaunchButton")]
		private Button launchButton;

		[SerializeField, Title("ErrorMessage")]
		private GameObject errorMessage; // used if weapons have not been equipped

		public ShipSelection ShipSelection { get; private set; }

		private void Awake()
		{
			ShipSelection = FindObjectOfType<ShipSelection>();
		}

		private void OnEnable()
		{
			// disable 'launch' button and show error if
			// placeholder weapons are equipped.
			foreach (WeaponEntity w in ShipSelection.CurrentShip.WeaponEntities)
			{
				if (w.WeaponData.Placeholder)
				{
					// not good to launch (placeholder weps still equipped)
					errorMessage.SetActive(true);
					launchButton.interactable = false;
					//highlightWeapons.SetActive(true);
					return;
				}
			}
			// good to launch
			errorMessage.SetActive(false);
			launchButton.interactable = true;
			//highlightWeapons.SetActive(false);
		}


	}
}
