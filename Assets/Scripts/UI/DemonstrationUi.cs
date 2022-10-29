using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DemonstrationUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] Toggle pause;
	[SerializeField] Transform pauseContent;
	[SerializeField] Button reset;
	[SerializeField] Slider densitySlider;
	[SerializeField] Slider heightSlider;

	public static bool IsMouseOver;

	public static event Action<bool> OnPause;

	public static IEnumerator FixUnityBug(bool value)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		print("Fix: " + value);
		IsMouseOver = value;
	}

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

	public void OnPointerEnter(PointerEventData eventData)
	{
		IsMouseOver = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		IsMouseOver = false;
	}

	// Events
	void Pause(bool value)
	{
		OnPause?.Invoke(value);
		pauseContent.gameObject.SetActive(value);
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
