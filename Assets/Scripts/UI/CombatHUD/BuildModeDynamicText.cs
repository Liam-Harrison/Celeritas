using Celeritas;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildModeDynamicText : MonoBehaviour
{
	InputAction BuildAction;
	TextMeshProUGUI textMesh;
	string StartText;
	string EndText;
	private void Start()
	{
		BuildAction = SettingsManager.InputActions.Basic.Build;
		textMesh = gameObject.GetComponent<TextMeshProUGUI>();
		StartText = textMesh.text.Substring(0, textMesh.text.IndexOf('[') + 1);
		EndText = textMesh.text.Substring(textMesh.text.IndexOf(']'));
		updateText();
	}

	private void updateText()
	{
		if (BuildAction != null)
			textMesh.SetText(StartText + BuildAction.GetBindingDisplayString().ToUpper() + EndText);
	}

	private void OnEnable()
	{
		SettingsManager.OnKeybindChanged += updateText;
		updateText();
	}

	private void OnDisable()
	{
		SettingsManager.OnKeybindChanged -= updateText;
		updateText();
	}
}
