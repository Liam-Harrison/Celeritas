using Celeritas.Game.Controllers;
using TMPro;
using UnityEngine;

public class RareMetalCounter : MonoBehaviour
{
	TextMeshProUGUI counter;

	private void Awake()
	{
		counter = gameObject.GetComponent<TextMeshProUGUI>();
		setCounterText(0, 0);
	}

	private void setCounterText(int type, int amount)
	{
		counter.SetText(LootController.Instance.RareMetals.ToString());
	}

	private void OnEnable()
	{
		LootController.OnRareComponentsChanged += setCounterText;
	}

	private void OnDisable()
	{
		LootController.OnRareComponentsChanged -= setCounterText;
	}
}