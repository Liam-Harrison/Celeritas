using Celeritas.Game.Entities;
using Celeritas.Scriptables;
using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeritas.UI.Runstart
{
	public class ShipSelection : MonoBehaviour
	{
		[SerializeField, TitleGroup("Rotation")]
		private Vector3 rotation;

		[SerializeField, TitleGroup("Rotation")]
		private AnimationCurve rotateCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

		[SerializeField, TitleGroup("Assignments")]
		private new CinemachineVirtualCamera camera;

		[SerializeField, TitleGroup("Spawn")]
		private Transform shipSpawn;

		/// <summary>
		/// The currently selected ship.
		/// </summary>
		public static PlayerShipEntity CurrentShip { get; private set; }

		/// <summary>
		/// The spawn transform of the ship.
		/// </summary>
		public Transform ShipSpawn { get => shipSpawn; }

		private readonly Dictionary<ShipData, PlayerShipEntity> shipObjects = new Dictionary<ShipData, PlayerShipEntity>();
		private readonly List<WeaponData> weaponList = new List<WeaponData>();

		private void Awake()
		{
			if (EntityDataManager.Instance != null && EntityDataManager.Instance.Loaded)
				SetupData();
			else
				EntityDataManager.OnLoadedAssets += SetupData;
		}

		private void OnEnable()
		{
			StopAllCoroutines();
			ShipSpawn.rotation = Quaternion.Euler(rotation);
		}

		private void Update()
		{
			if (CurrentShip != null)
			{
				CurrentShip.transform.rotation = ShipSpawn.rotation;
			}
		}

		/// <summary>
		/// Select a specific player ship.
		/// </summary>
		/// <param name="ship">The player ship data to select.</param>
		public void SelectShip(ShipData ship)
		{
			if (shipObjects.Count == 0)
				SetupData();

			if (!shipObjects.ContainsKey(ship))
				return;

			if (CurrentShip != null)
				CurrentShip.gameObject.SetActive(false);

			CurrentShip = shipObjects[ship];
			camera.m_Lens.OrthographicSize = CurrentShip.SelectionViewSize;

			CurrentShip.gameObject.SetActive(true);
		}

		/// <summary>
		/// Rotate the ship spawn to the provided rotation.
		/// </summary>
		/// <param name="rotation">The rotation to move to.</param>
		public void RotateOrigin(Vector3 rotation)
		{
			StopAllCoroutines();
			StartCoroutine(RotateCoroutine(rotation));
		}

		public void Launch()
		{
			StopAllCoroutines();

			foreach (var ship in shipObjects)
			{
				if (ship.Value != CurrentShip)
					EntityDataManager.UnloadEntity(ship.Value);
			}
			shipObjects.Clear();

			CurrentShip.transform.rotation = Quaternion.identity;
			CurrentShip.IsStationary = false;

			GetComponent<SceneLoader>().LoadScene();
		}

		private IEnumerator RotateCoroutine(Vector3 rotation)
		{
			float start = Time.time;
			var q = ShipSpawn.rotation;
			float time = 0.2f;
			float p;

			do
			{
				p = Mathf.Clamp01(rotateCurve.Evaluate((Time.time - start) / time));
				ShipSpawn.rotation = Quaternion.Slerp(q, Quaternion.Euler(rotation), p);
				yield return null;
			} while (p < 1);

			ShipSpawn.rotation = Quaternion.Euler(rotation);

			yield break;
		}

		private void SetupData()
		{
			EntityDataManager.OnLoadedAssets -= SetupData;

			if (shipObjects.Count > 0)
				return;

			foreach (ShipData data in EntityDataManager.Instance.PlayerShips)
			{
				var ship = EntityDataManager.InstantiateEntity<PlayerShipEntity>(data, forceIsPlayer: true);

				ship.IsStationary = true;
				ship.gameObject.SetActive(false);

				shipObjects.Add(data, ship);
			}

			foreach (WeaponData weapon in EntityDataManager.Instance.Weapons)
			{
				weaponList.Add(weapon);
			}
		}
	}
}