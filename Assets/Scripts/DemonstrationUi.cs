using UnityEngine;
using UnityEngine.UI;

public class DemonstrationUi : MonoBehaviour
{
	[SerializeField] Button reset;
	[SerializeField] Toggle pause;
	[SerializeField] Slider densitySlider;
	[SerializeField] Slider heightSlider;

	void CallAllFunctions()
	{
		DensityChanged(densitySlider.value, false);
		HeightChanged(heightSlider.value, false);

		BallSpawner.Instance.RespawnLevel();
	}

	// Unity
	private void Start()
	{
		// Updating level with ui
		CallAllFunctions();

		reset.onClick.AddListener(() => BallSpawner.Instance.RespawnLevel());
		pause.onValueChanged.AddListener(Pause);
		densitySlider.onValueChanged.AddListener(value => DensityChanged(value));
		heightSlider.onValueChanged.AddListener(value => HeightChanged(value));
	}

	// Events
	void Pause(bool value)
	{

	}

	void DensityChanged(float value, bool respawnLevel = false)
	{
		BallSpawner.Instance.SetDebsity((int)value);
		if (respawnLevel) BallSpawner.Instance.RespawnLevel();
	}

	void HeightChanged(float value, bool respawnLevel = false)
	{
		BallSpawner.Instance.SetHeight((int)value);
		if (respawnLevel) BallSpawner.Instance.RespawnLevel();
	}
}
