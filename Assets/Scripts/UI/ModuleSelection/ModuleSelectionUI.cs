using Celeritas.Scriptables;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Celeritas.UI
{
	/// <summary>
	/// Manages the selection UI in the Build HUD.
	/// </summary>
	public class ModuleSelectionUI : MonoBehaviour
	{
		[SerializeField, Title("Assignments")]
		private GameObject buttonPrefab;

		[SerializeField]
		private Transform parent;

		/// <summary>
		/// Present the user with a range of module options, invokes the provided callback when the user inputs a selection.
		/// </summary>
		/// <param name="modules">The modules to present to the user.</param>
		/// <param name="callback">The invoked callback when selected.</param>
		public void SelectOption(ModuleData[] modules, Action<ModuleData> callback)
		{
			gameObject.SetActive(true);

			for (int i = 0; i < parent.childCount; i++)
			{
				Destroy(parent.GetChild(i).gameObject);
			}

			foreach (var module in modules)
			{
				var item = Instantiate(buttonPrefab, parent).GetComponent<ModuleSelectionItem>();

				item.Label = module.Title;
				item.Image.sprite = module.Icon;
				item.Module = module;
				item.Button.onClick.AddListener(
				() => {
					gameObject.SetActive(false);
					callback?.Invoke(module);
				});
			}
		}
	}
}