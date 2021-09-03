using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.RunStart
{
	/// <summary>
	/// Makes it easy to access and change elements of the ship's stat preview
	/// One will exist for each stat. Eg, one for health, one for speed, ect
	/// </summary>
	class ShipSelectionStats : MonoBehaviour
	{
		[SerializeField]
		public TextMeshProUGUI title;

		[SerializeField]
		public TextMeshProUGUI titleUnderlay;

		[SerializeField]
		public Slider slider;

		[SerializeField]
		public GameObject sliderObject;

		[SerializeField]
		public Image rectFill;

		[SerializeField]
		Gradient colourGradient;

		public void hideSlider()
		{
			sliderObject.SetActive(false);
		}

		public void setSliderValue(float value)
		{
			slider.value = value;
			rectFill.color = colourGradient.Evaluate(value / slider.maxValue);
		}

		public void setTitle(string titleText)
		{
			title.text = titleText;
			if (titleUnderlay != null)
				titleUnderlay.text = titleText;
		}
	}
}
