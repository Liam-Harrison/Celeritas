using Assets.Scripts.Game.Controllers;
using Celeritas.Game.Actions;
using Celeritas.Game.Entities;
using System;
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
			public GameAction primary;
			public GameAction alternate;
		}

		private InputActions.BasicActions actions = default;
		private Camera _camera;

		private readonly BindedAbilities[] abilities = new BindedAbilities[4];


		/// <summary>
		/// The ship entity that this controller is attatched to.
		/// </summary>
		public PlayerShipEntity PlayerShipEntity { get; private set; }

		public bool LockInput { get; set; }

		private bool IsAlternateMode { get; set; }

		TractorBeamController tractorBeam;

		public static event Action OnPlayerShipCreated;

		public static event Action<int, bool, GameAction> OnActionLinked;
		public static event Action<int, bool> OnActionUnlinked;

		protected override void Awake()
		{
			actions = new InputActions.BasicActions(SettingsManager.InputActions);
			actions.SetCallbacks(this);

			PlayerShipEntity = GetComponent<PlayerShipEntity>();
			_camera = Camera.main;

			for (int i = 0; i < abilities.Length; i++)
			{
				abilities[i] = new BindedAbilities();
			}

			base.Awake();

			foreach (var action in PlayerShipEntity.Actions)
			{
				BindAction(action);
			}
			OnPlayerShipCreated?.Invoke();
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
			if (GameStateManager.Instance.GameState == GameState.BUILD || LockInput)
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

		public void BindAction(GameAction action)
		{
			bool isAlt = false;
			for (int i = 0; i < abilities.Length; i++)
			{
				if (isAlt && abilities[i].primary == null)
				{
					BindAction(i, isAlt, action);
					return;
				}
				else if (abilities[i].alternate == null)
				{
					BindAction(i, isAlt, action);
					return;
				}
			}
		}

		public void BindAction(int index, bool isAlternate, GameAction action)
		{
			if (isAlternate)
				abilities[index].alternate = action;
			else
				abilities[index].primary = action;

			OnActionLinked?.Invoke(index, isAlternate, action);
		}

		private void UnbindAction(int index, bool isAlternate)
		{
			if (isAlternate)
				abilities[index].alternate = null;
			else
				abilities[index].primary = null;

			OnActionUnlinked?.Invoke(index, isAlternate);
		}

		public bool TryGetBindedAction(int index, bool isAlternate, out GameAction action)
		{
			if (isAlternate)
				action = abilities[index].alternate;
			else
				action = abilities[index].primary;

			return action != null;
		}

		public void OnFire(InputAction.CallbackContext context)
		{
			if (GameStateManager.Instance.GameState == GameState.BUILD || LockInput)
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
			if (context.performed && WaveManager.Instance.WaveActive == false && LockInput == false)
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
			UseAbility(abilities[0]);
		}

		public void OnAbility2(InputAction.CallbackContext context)
		{
			UseAbility(abilities[1]);
		}

		public void OnAbility3(InputAction.CallbackContext context)
		{
			UseAbility(abilities[2]);
		}

		public void OnAbility4(InputAction.CallbackContext context)
		{
			UseAbility(abilities[3]);
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
			if (IsAlternateMode == false && ability.primary != null)
					ability.primary.ExecuteAction(PlayerShipEntity);
			else if (ability.alternate != null)
				ability.alternate.ExecuteAction(PlayerShipEntity);
		}
	}
}
