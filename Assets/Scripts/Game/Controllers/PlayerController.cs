using Assets.Scripts.Game.Controllers;
using Celeritas.Game.Entities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeritas.Game.Controllers
{
	/// <summary>
	/// Routes player input to a targeted ship Entity.
	/// </summary>
	[RequireComponent(typeof(PlayerShipEntity))]
	public class PlayerController : Singleton<PlayerController>, InputActions.IBasicActions
	{
		private InputActions.BasicActions actions = default;
		private Camera _camera;

		/// <summary>
		/// The ship entity that this controller is attatched to.
		/// </summary>
		public PlayerShipEntity PlayerShipEntity { get; private set; }

		protected override void Awake()
		{
			actions = new InputActions.BasicActions(SettingsManager.InputActions);
			actions.SetCallbacks(this);

			PlayerShipEntity = GetComponent<PlayerShipEntity>();

			_camera = Camera.main;

			base.Awake();
		}

		protected virtual void OnEnable()
		{
			actions.Enable();
		}

		protected virtual void OnDisable()
		{
			actions.Disable();
		}

		private Vector2 locomotion;

		protected void Update()
		{
			if (GameStateManager.Instance.GameState == GameState.BUILD)
			{
				PlayerShipEntity.AimTarget = transform.position + PlayerShipEntity.Forward * 50f;
				PlayerShipEntity.Translation = Vector3.zero;
			}
			else
			{
				var target = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
				PlayerShipEntity.AimTarget = Vector3.ProjectOnPlane(target, Vector3.forward);
				PlayerShipEntity.Translation = locomotion;
			}
		}

		public void OnFire(InputAction.CallbackContext context)
		{
			if (GameStateManager.Instance.GameState == GameState.BUILD)
				return;

			if (context.performed)
			{
				foreach (var weapon in PlayerShipEntity.WeaponEntities)
				{
					weapon.Firing = true;
				}
			}

			if (context.canceled)
			{
				foreach (var weapon in PlayerShipEntity.WeaponEntities)
				{
					weapon.Firing = false;
				}
			}
		}

		public void OnBuild(InputAction.CallbackContext context)
		{
			if (context.canceled && !WaveManager.Instance.WaveActive)
			{
				if (GameStateManager.Instance.GameState == GameState.BACKGROUND)
				{
					GameStateManager.Instance.SetGameState(GameState.BUILD);
				}
				else
				{
					GameStateManager.Instance.SetGameState(GameState.BACKGROUND);
				}
			}
		}

		public void OnAction(InputAction.CallbackContext context)
		{
			PlayerShipEntity.UseActions();
		}

		TractorBeamController tractorBeam;

		/// <summary>
		/// Toggles tractorActive on/off when corrosponding input is pressed/released.
		/// Also finds the closest entity within range, sets it as tractorTarget
		/// </summary>
		/// <param name="context"></param>
		public void OnTractorBeam(InputAction.CallbackContext context)
		{
			if (tractorBeam == null && TractorBeamController.Instance != null)
				tractorBeam = TractorBeamController.Instance;

			if (tractorBeam != null)
			{
				tractorBeam = TractorBeamController.Instance;
				tractorBeam.OnTractorBeam(context, PlayerShipEntity);
			}
		}

		public void OnNewWave(InputAction.CallbackContext context)
		{
			if (GameStateManager.Instance.GameState == GameState.BUILD)
				return;

			if (context.performed)
			{
				WaveManager.Instance.StartRandomWave();
			}
		}
		public void OnFinalWave(InputAction.CallbackContext context)
		{
			if (GameStateManager.Instance.GameState == GameState.BUILD)
				return;

			if (context.performed)
			{
				WaveManager.Instance.StartFinalWave();
			}
		}



		public void OnToggleTutorial(InputAction.CallbackContext context)
		{
			CombatHUD.Instance.OnToggleTutorial();
		}

		public void OnMove(InputAction.CallbackContext context)
		{
			locomotion = context.ReadValue<Vector2>();
		}
	}
}
