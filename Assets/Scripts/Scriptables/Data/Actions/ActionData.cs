using System;
using UnityEngine;

namespace Celeritas.Scriptables
{
	/// <summary>
	/// Base class for all action scriptable objects.
	/// </summary>
	public abstract class ActionData : ScriptableObject
	{
		[SerializeField]
		private string title;

		[SerializeField]
		private Sprite icon;

		[SerializeField]
		private float cooldownSeconds;

		[SerializeField]
		private bool isPassive;

		/// <summary>
		/// The title of this action.
		/// </summary>
		public string Title { get => title; }

		/// <summary>
		/// The cooldown in seconds for this action.
		/// </summary>
		public float CooldownSeconds { get => cooldownSeconds; }

		/// <summary>
		/// The icon for this action.
		/// </summary>
		public Sprite Icon { get => icon; }

		/// <summary>
		/// Is this a passive action.
		/// </summary>
		public bool IsPassive {  get => isPassive; }

		/// <summary>
		/// The type of this action.
		/// </summary>
		public abstract Type ActionType { get; }

		public abstract string GetTooltip(int level);
	}
}
