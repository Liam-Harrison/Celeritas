using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAbilitySlot : MonoBehaviour
{
	private DummyAbility ability; // that is stored in this ability slot

	public DummyAbility Ability { get => ability; set => ability = value; }

	public string InputButtonText { get => ability.inputButton; set => ability.inputButton = value; }

	public Sprite Icon { get => ability.icon; set => ability.icon = value; }

	// todo: icon

	/// <summary>
	/// Setup gameobject using current Ability information
	/// </summary>
	public void Initalize()
	{
		gameObject.SetActive(true);
		Text inputButtonText = gameObject.GetComponentInChildren<Text>();
		inputButtonText.text = InputButtonText;
		//Image image = gameObject.GetComponentInChildren<Image>();
		GameObject image = transform.Find("Icon").gameObject;
		image.GetComponent<Image>().sprite = Icon;
	}

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
