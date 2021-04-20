//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssetIcons.Editors.Preferences
{
	/// <summary>
	/// <para>Provides a serializable dictionary for mapping an extension type to an <see cref="PreferencesAssetReference"/>.</para>
	/// </summary>
	/// <seealso cref="AssetIconsPreferencesPreset"/>
	/// <seealso cref="PreferencesAssetReference"/>
	[Serializable]
	public sealed class IconMapping : ISerializationCallbackReceiver
	{
		[Serializable]
		private struct TableEntry
		{
			public string key;
			public PreferencesAssetReference value;

			public TableEntry(string key, PreferencesAssetReference value)
			{
				this.key = key;
				this.value = value;
			}
		}

#pragma warning disable RECS0092 // Convert field to readonly

		[SerializeField]
		private List<TableEntry> keyValuePairs = new List<TableEntry>();

#pragma warning restore RECS0092 // Convert field to readonly

		private readonly Dictionary<string, PreferencesAssetReference> dictionary = new Dictionary<string, PreferencesAssetReference>();

		/// <summary>
		/// <para>An event that fires whenever the content of the table changes.</para>
		/// </summary>
		public event Action OnChanged;

		/// <summary>
		/// <para>Returns the value in the table at the specified key.</para>
		/// </summary>
		/// <param name="key">The key of the value to retrieve.</param>
		/// <returns>
		/// <para>The value with the specified key.</para>
		/// </returns>
		public PreferencesAssetReference this[string key]
		{
			get
			{
				PreferencesAssetReference value;
				dictionary.TryGetValue(key, out value);
				return value;
			}
			set
			{
				int modifyIndex = IndexOfKey(key);
				if (modifyIndex == -1)
				{
					Add(key, value);
				}

				dictionary[key] = value;
				keyValuePairs[modifyIndex] = new TableEntry(key, value);
				InvokeChanged();
			}
		}

		/// <summary>
		/// <para>Add a key and a value to the <see cref="IconMapping" />.</para>
		/// </summary>
		/// <param name="key">The key of the new value.</param>
		/// <param name="value">The value to add to the table.</param>
		public void Add(string key, PreferencesAssetReference value)
		{
			if (IndexOfKey(key) == -1)
			{
				throw new ArgumentException("An item with the same key has already been added.");
			}

			dictionary.Add(key, value);
			keyValuePairs.Add(new TableEntry(key, value));
			InvokeChanged();
		}

		/// <summary>
		/// <para>Clear all values from the <see cref="IconMapping" />.</para>
		/// </summary>
		public void Clear()
		{
			dictionary.Clear();
			keyValuePairs.Clear();
			InvokeChanged();
		}


		/// <summary>
		/// <para>Removes an element from the table using a key.</para>
		/// </summary>
		/// <param name="key">The key of the value to remove.</param>
		/// <returns>
		/// <para><c>true</c> if the element was successfully removed from the table; otherwise, <c>false</c>.</para>
		/// </returns>
		public bool Remove(string key)
		{
			int removeIndex = IndexOfKey(key);
			if (removeIndex == -1)
			{
				return false;
			}

			dictionary.Remove(key);
			keyValuePairs.RemoveAt(removeIndex);
			InvokeChanged();
			return true;
		}

		/// <summary>
		/// <para>Implements the Unity <see cref="ISerializationCallbackReceiver"/> interface.</para>
		/// </summary>
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			dictionary.Clear();

			for (int i = 0; i < keyValuePairs.Count; i++)
			{
				dictionary[keyValuePairs[i].key] = keyValuePairs[i].value;
			}
		}

		/// <summary>
		/// <para>Implements the Unity <see cref="ISerializationCallbackReceiver"/> interface.</para>
		/// </summary>
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		private void InvokeChanged()
		{
			if (OnChanged != null)
			{
				OnChanged();
			}
		}

		private int IndexOfKey(string key)
		{
			for (int i = 0; i < keyValuePairs.Count; i++)
			{
				string tableKey = keyValuePairs[i].key;

				if (tableKey == key)
				{
					return i;
				}
			}
			return -1;
		}
	}
}

#pragma warning restore
#endif
