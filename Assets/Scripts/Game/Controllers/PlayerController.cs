using Assets.Scripts.Game.Controllers;
using Celeritas.Game.Actions;
using Celeritas.Game.Entities;
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
		public class BindedAbilities
		{
			public GameAction ability;
			public GameAction alternate;
		}

		private InputActions.BasicActions actions = default;
		private Camera _camera;

		private readonly BindedAbilities ability_one = new BindedAbilities();
		private readonly BindedAbilities ability_two = new BindedAbilities();
		private readonly BindedAbilities ability_three = new BindedAbilities();
		private readonly BindedAbilities ability_four = new BindedAbilities();

		/// <summary>
		/// The ship entity that this controller is attatched to.
		/// </summary>
		public PlayerShipEntity PlayerShipEntity { get; private set; }

		private bool IsAlternateMode { get; set; }

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
			if (context.performed && !WaveManager.Instance.WaveActive)
			{
				if (GameStateManager.Instance.GameState == GameState.BACKGROUND)
				{
					GameStateManager.Instance.SetGameState(GameState.BUILD);
				}
				else if (GameStateManager.Instance.GameState == GameState.BUILD)
				{
					GameStateManager.Instance.SetGameState(GameState.BACKGROUND);

					Instance.actions = new InputActions.BasicActions(SettingsManager.InputActions);
					Instance.actions.SetCallbacks(Instance);
					Instance.actions.Enable();
				}
			}
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

		public void OnToggleTutorial(InputAction.CallbackContext context)
		{
			CombatHUD.Instance.OnToggleTutorial();
		}

		public void OnMove(InputAction.CallbackContext context)
		{
			locomotion = context.ReadValue<Vector2>();
		}

		public void OnAbility1(InputAction.CallbackContext context)
		{
			UseAbility(ability_one);
		}

		public void OnAbility2(InputAction.CallbackContext context)
		{
			UseAbility(ability_two);
		}

		public void OnAbility3(InputAction.CallbackContext context)
		{
			UseAbility(ability_three);
		}

		public void OnAbility4(InputAction.CallbackContext context)
		{
			UseAbility(ability_four);
		}

		public void OnAlternateAbilities(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				IsAlternateMode = true;
			}
			else if (context.canceled)
			{
				IsAlternateMode = false;
			}
		}

		private void UseAbility(BindedAbilities ability)
		{
			if (IsAlternateMode == false)
			{
				ability.ability.ExecuteAction(PlayerShipEntity);
			}
			else
			{
				ability.alternate.ExecuteAction(PlayerShipEntity);
			}
		}
	}
}
