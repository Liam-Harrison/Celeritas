using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a single square that displays ability icon/controls in the HUD.
/// Includes what control must be pressed to activate it, a representative icon, and (pending) cooldown.
/// </summary>
public class UIAbilitySlot : MonoBehaviour
{
	private DummyAbility ability; // that is stored in this ability slot

	/// <summary>
	/// The ability that is being represented by this HUD element
	/// </summary>
	public DummyAbility Ability { get => ability; set => ability = value; }

	/// <summary>
	/// Used to display what input button should be used to activate the ability
	/// </summary>
	public string InputButtonText { get => ability.inputButton; set => ability.inputButton = value; }

	/// <summary>
	/// The icon associated with the ability
	/// </summary>
	public Sprite Icon { get => ability.icon; set => ability.icon = value; }

	/// <summary>
	/// Setup gameobject using current Ability information
	/// </summary>
	public void Initalize()
	{
		gameObject.SetActive(true);
		Text inputButtonText = gameObject.GetComponentInChildren<Text>();
		inputButtonText.text = InputButtonText;
		GameObject image = transform.Find("Icon").gameObject;
		image.GetComponent<Image>().sprite = Icon;
	}

	/// <summary>
	/// Used when the Slot has no active ability to display
	/// Makes the ability slot invisible
	/// </summary>
	public void Empty()
	{
		gameObject.SetActive(false);
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
