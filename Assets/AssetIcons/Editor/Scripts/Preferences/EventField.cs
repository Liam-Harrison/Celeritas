//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace AssetIcons.Editors.Preferences
{
	/// <summary>
	/// <para>A base-class for the generic wrapper object that fires events whenever its value changes.</para>
	/// </summary>
	public abstract class EventField
	{
		/// <summary>
		/// <para>Event that is fired whenever the value of this <see cref="EventField"/> changes.</para>
		/// </summary>
		public event Action OnChanged;

		/// <summary>
		/// <para>Invokes the <see cref="OnChanged"/> event.</para>
		/// </summary>
		/// <event cref="OnChanged">Fired upon calling this method.</event>
		protected void InvokeChanged()
		{
			if (OnChanged != null)
			{
				OnChanged();
			}
		}
	}

	/// <summary>
	/// <para>A generic wrapper object that fires events whenever its value changes.</para>
	/// </summary>
	/// <seealso cref="BoolEventField"/>
	/// <seealso cref="IntEventField"/>
	public class EventField<T> : EventField, ISerializationCallbackReceiver
	{
		[SerializeField]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private T internalValue;

		[SerializeField]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private T beforeSerialization;

		/// <summary>
		/// <para>The value that this wrapper currently represents.</para>
		/// </summary>
		/// <event cref="EventField.OnChanged">Fired whenever the value of this property change.</event>
		public T Value
		{
			get
			{
				return internalValue;
			}
			set
			{
				if (!EqualityComparer<T>.Default.Equals(internalValue, value))
				{
					internalValue = value;

					InvokeChanged();
				}
			}
		}

		/// <summary>
		/// <para>Constructs a new instance of this <see cref="EventField"/>.</para>
		/// </summary>
		public EventField()
		{
		}

		/// <summary>
		/// <para>Constructs a new instance of this <see cref="EventField"/> with a default value.</para>
		/// </summary>
		/// <param name="defaultValue">The default value for this <see cref="BoolEventField"/>.</param>
		public EventField(T defaultValue)
		{
			internalValue = defaultValue;
		}

		/// <summary>
		/// <para>Implements the Unity <see cref="ISerializationCallbackReceiver"/> interface.</para>
		/// </summary>
		public void OnAfterDeserialize()
		{
			if (!EqualityComparer<T>.Default.Equals(internalValue, beforeSerialization))
			{
				InvokeChanged();
			}
		}

		/// <summary>
		/// <para>Implements the Unity <see cref="ISerializationCallbackReceiver"/> interface.</para>
		/// </summary>
		public void OnBeforeSerialize()
		{
			beforeSerialization = internalValue;
		}
	}
}

#pragma warning restore
#endif
