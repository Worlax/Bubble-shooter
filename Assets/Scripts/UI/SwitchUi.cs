using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwitchUi : MonoBehaviour
{
	[SerializeField] DemonstrationUi demonstrationUi;
	[SerializeField] Button button;
	[SerializeField] Transform ui;

	IEnumerator FixUnityBug()
	{
		if (ui.gameObject.activeSelf)
		{
			demonstrationUi.OnPointerExit(null);
		}
		else
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			demonstrationUi.OnPointerEnter(null);
		}
	}

	// Unity
	private void Start()
	{
		button.onClick.AddListener(Switch);
	}

	// Events
	void Switch()
	{
		// Unity bug... Event "OnPointerEnter" never triggers again
		// if gameObject is disabled once. So I'm manually setting the flag
		// when main menu is active.
		StartCoroutine(FixUnityBug());
		// ...

		BallSpawner.Instance.RespawnLevel();
		ui.gameObject.SetActive(!ui.gameObject.activeSelf);
	}
}
