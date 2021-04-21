using Celeritas.Game.Actions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

/// <summary>
/// Represents a single square that displays ability icon/controls in the HUD.
/// Includes what control must be pressed to activate it, a representative icon, and (pending) cooldown.
/// </summary>
public class UIAbilitySlot : MonoBehaviour
{
	/// <summary>
	/// Get the action linked to this UI slot.
	/// </summary>
	public GameAction LinkedAction { get; private set; }

	[SerializeField, Title("Assignments")]
	private Image icon;

	[SerializeField]
	private TextMeshProUGUI label;

	[SerializeField]
	private TextMeshProUGUI key;

	[SerializeField]
	private TextMeshProUGUI time;

	/// <summary>
	/// Link this slot to an action.
	/// </summary>
	/// <param name="action">The action to link this to.</param>
	public void LinkToAction(GameAction action)
	{
		LinkedAction = action;

		icon.sprite = action.Data.Icon;
		label.text = action.Data.Title;
		key.text = "F";
		time.text = "0.0";
	}

	/// <summary>
	/// Used when the Slot has no active ability to display
	/// Makes the ability slot invisible
	/// </summary>
	public void Hide()
	{
		gameObject.SetActive(false);
	}

	private void Update()
	{
		if (LinkedAction == null)
			return;

		time.text = $"{LinkedAction.TimeLeftOnCooldown:0.0}";
	}
}
