using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SampleUIController : MonoBehaviour {

	private static SampleUIController instance = null;

	public Text someText;
	public Button button1;
	public Button button2;
	public Button button3;

	void Awake() {
		instance = this;
	}

	[Calabash.Backdoor("EnableCanvasScaling")]
	public static void EnableCanvasScaling() {
		instance.GetComponent<CanvasScaler>().enabled = true;
	}

	[Calabash.Backdoor("EnableCanvasCamera")]
	public static void EnableCanvasCamera() {
		instance.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
		instance.GetComponent<Canvas>().worldCamera = Camera.main;
	}

	public void Button1ClickHandler() {
		if (!button2.isActiveAndEnabled) {
			button2.gameObject.SetActive(true);
		} else {
			button3.gameObject.SetActive(true);
		}
	}

	public void Button2ClickHandler() {
		button2.GetComponentInChildren<Text>().text = "NEW TEXT";
	}

	public void Button3ClickHandler() {
		someText.gameObject.SetActive(true);
	}
}
