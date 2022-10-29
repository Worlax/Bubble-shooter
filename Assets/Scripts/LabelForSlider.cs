using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabelForSlider : MonoBehaviour
{
	[SerializeField] string startText;
	[SerializeField] TMP_Text text;
	[SerializeField] Slider slider;

	// Unity
	private void Start()
	{
		slider.onValueChanged.AddListener(SliderValueChanged);
		SliderValueChanged(slider.value);
	}

	// Events
	void SliderValueChanged(float value)
	{
		text.text = startText + value;
	}
}
