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
		/// The type of this action.
		/// </summary>
		public abstract Type ActionType { get; }
	}
}
