using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Celeritas.UI.Dialogue
{
	public enum OptionType
	{
		Neutral,
		Good,
		Bad,
	}

	public class DialogueOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
	{
		[SerializeField, TitleGroup("Assignments")]
		private TextMeshProUGUI label;

		[SerializeField, TabGroup("Neutral")]
		private Color neutralNormal = Color.white;

		[SerializeField, TabGroup("Neutral")]
		private Color neutralHighlighted = Color.white;

		[SerializeField, TabGroup("Neutral")]
		private Color neutralPressed = Color.white;

		[SerializeField, TabGroup("Good")]
		private Color goodNormal = Color.white;

		[SerializeField, TabGroup("Good")]
		private Color goodHighlighted = Color.white;

		[SerializeField, TabGroup("Good")]
		private Color goodPressed = Color.white;

		[SerializeField, TabGroup("Bad")]
		private Color badNormal = Color.white;

		[SerializeField, TabGroup("Bad")]
		private Color badHighlighted = Color.white;

		[SerializeField, TabGroup("Bad")]
		private Color badPressed = Color.white;

		public string Text { get => label.text; set => label.text = value; }

		public OptionType OptionType { get; private set; }

		private bool mouseOver = false;
		private bool isPressed = false;

		private Color normal = Color.white;

		private Color highlighted = Color.white;

		private Color pressed = Color.white;

		public event Action OnClicked;

		public void SetOptionType(OptionType type)
		{
			OptionType = type;
			switch (type)
			{
				case OptionType.Neutral:
					SetColors(neutralNormal, neutralHighlighted, neutralPressed);
					break;
				case OptionType.Good:
					SetColors(goodNormal, goodHighlighted, goodPressed);
					break;
				case OptionType.Bad:
					SetColors(badNormal, badHighlighted, badPressed);
					break;
				default:
					SetColors(neutralNormal, neutralHighlighted, neutralPressed);
					break;
			}
		}

		private void SetColors(Color normal, Color highlighted, Color pressed)
		{
			this.normal = normal;
			this.highlighted = highlighted;
			this.pressed = pressed;
			RefreshColors();
		}

		private void RefreshColors()
		{
			if (isPressed)
				label.color = pressed;
			else if (mouseOver)
				label.color = highlighted;
			else
				label.color = normal;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			OnClicked?.Invoke();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			isPressed = true;
			RefreshColors();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			mouseOver = true;
			RefreshColors();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			mouseOver = false;
			isPressed = false;
			RefreshColors();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			isPressed = false;
			RefreshColors();
		}
	}
}