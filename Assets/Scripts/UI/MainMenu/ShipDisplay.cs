using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Cinemachine;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace Celeritas.UI
{
	public class ShipDisplay : MonoBehaviour
	{
		[SerializeField, TitleGroup("Assignments")]
		private new CinemachineVirtualCamera camera;

		/// <summary>
		/// The currently selected ship.
		/// </summary>
		public static PlayerShipEntity CurrentShip { get; private set; }

		private void Awake()
		{
			if (EntityDataManager.Instance != null && EntityDataManager.Instance.Loaded)
				SelectShip();
			else
				EntityDataManager.OnLoadedAssets += SelectShip;
		}

		private void OnDestroy()
		{
#if UNITY_EDITOR
			if (UnityEditor.EditorApplication.isPlaying == false)
				return;
#endif

			if (CurrentShip != null)
				CurrentShip.UnloadEntity();

			CurrentShip = null;
		}

		private void SelectShip()
		{
			var ship = EntityDataManager.Instance.PlayerShips.ElementAt(Random.Range(0, EntityDataManager.Instance.PlayerShips.Count - 1));
			SelectShip(ship);
		}

		/// <summary>
		/// Select a specific player ship.
		/// </summary>
		/// <param name="ship">The player ship data to select.</param>
		public void SelectShip(ShipData ship)
		{
			CurrentShip = EntityDataManager.InstantiateEntity<PlayerShipEntity>(ship, forceIsPlayer: true, dontPool: true);

			CurrentShip.IsStationary = true;
			CurrentShip.transform.parent = transform;
			CurrentShip.transform.localRotation = Quaternion.identity;
			CurrentShip.transform.localPosition = Vector3.zero;

			camera.m_Lens.OrthographicSize = CurrentShip.SelectionViewSize;
		}
	}
}