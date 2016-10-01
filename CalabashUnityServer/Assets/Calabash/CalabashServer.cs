using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using Calabash;
using System.Threading;

[Serializable]
public struct CalabashMatchedObject {
	public string label;
	public Calabash.Rect rect;
}

public class CalabashServer : MonoBehaviour {

	private Calabash.HttpServer httpServer = null;

	// file used for logging
	public string logFile;

	public static CalabashServer Instance { get; private set; }

	void Awake() {
		#if CALABASH_UNITY
		Instance = this;
		httpServer = new Calabash.HttpServer();
		#endif
	}

	void Start() {
		#if CALABASH_UNITY
		httpServer.Start();
		#endif
	}

	void Destroy() {
		#if CALABASH_UNITY
		httpServer.Destroy();
		httpServer = null;
		#endif
	}

	private readonly LinkedList<Task> queue = new LinkedList<Task>();

	public Task<T> ExecuteOnMainThread<T>(Func<T> action) {
		Task<T> t = new Task<T>(action);
		lock (queue) {
			queue.AddLast(t);
		}
		return t;
	}

	public Task ExecuteOnMainThread(Action action) {
		Task t = new Task(action);
		lock (queue) {
			queue.AddLast(t);
		}
		return t;
	}

	public void Update() {
		ExecutedPendingTasks();
	}

	private void ExecutedPendingTasks()
	{
		while (true)
		{
			Task task;
			lock (queue)
			{
				if (queue.Count == 0)
				{
					break;
				}
				task = queue.First.Value;
				queue.RemoveFirst();
			}

			if (task != null)
			{
				task.Do();
			}
		}
	}

	public Calabash.Rect GetUIRect(GameObject gameObject, UnityEngine.Canvas canvas) {
		var rectTransform = gameObject.GetComponent<RectTransform>();
		Vector3[] v = new Vector3[4];
		rectTransform.GetWorldCorners(v);

		FileLog.Log(v[0].x + " " + v[0].y + " " + v[0].z);
		FileLog.Log(v[1].x + " " + v[1].y + " " + v[1].z);
		FileLog.Log(v[2].x + " " + v[2].y + " " + v[2].z);
		FileLog.Log(v[3].x + " " + v[3].y + " " + v[3].z);

		FileLog.Log("CANVAS");
		FileLog.Log(canvas.GetComponent<RectTransform>().rect.x.ToString());
		FileLog.Log(canvas.GetComponent<RectTransform>().rect.y.ToString());
		FileLog.Log(canvas.GetComponent<RectTransform>().rect.width.ToString());
		FileLog.Log(canvas.GetComponent<RectTransform>().rect.height.ToString());
		FileLog.Log(canvas.GetComponent<RectTransform>().localScale.x.ToString());
		FileLog.Log(canvas.GetComponent<RectTransform>().localScale.y.ToString());
		FileLog.Log("RESOLUTION");
		FileLog.Log(UnityEngine.Screen.currentResolution.width.ToString());
		FileLog.Log(UnityEngine.Screen.currentResolution.height.ToString());

		float scale = 0.0f;
		float screenWidth = 375.0f;

		if (Calabash.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadAir1) {
			screenWidth = 768.0f;
		}

		if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight) {
			screenWidth = 667.0f;

			if (Calabash.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadAir1) {
				screenWidth = 1024.0f;
			}
		}

		if (canvas.renderMode == RenderMode.ScreenSpaceCamera) {
			FileLog.Log("CANVAS IN CAMERA MODE");
			FileLog.Log(canvas.worldCamera.orthographicSize.ToString());
			FileLog.Log(canvas.worldCamera.aspect.ToString());

			float cameraSize = canvas.worldCamera.orthographicSize;

			bool bb = false;
			bb |= Calabash.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadAir1;
			bb |= Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight;
			if (bb) {
				cameraSize = 2.0f * canvas.worldCamera.orthographicSize * canvas.worldCamera.aspect;
			}
				
			for (int i = 0; i < 4; i++) {
				v[i].x = v[i].x + 0.5f * cameraSize;
				v[i].y = -v[i].y + 0.5f * cameraSize / canvas.worldCamera.aspect;
			}

			scale = screenWidth / cameraSize;
		} else {
			float worldHeight = UnityEngine.Screen.currentResolution.height;

			for (int i = 0; i < 4; i++) {
				v[i].y = worldHeight - v[i].y;
			}

			scale = screenWidth / UnityEngine.Screen.currentResolution.width;
		}

		Calabash.Rect result = new Calabash.Rect { 
			x = v[0].x,
			y = v[1].y, 
			width = v[2].x - v[0].x,
			height = v[0].y - v[1].y
		};

		result.x = scale * result.x;
		result.y = scale * result.y;
		result.width = scale * result.width;
		result.height = scale * result.height;

		result.center_x = result.x + 0.5f * result.width;
		result.center_y = result.y + 0.5f * result.height;

		return result;
	}

	public List<CalabashMatchedObject> Query(string queryString) {
		List<CalabashMatchedObject> results = new List<CalabashMatchedObject>();

		Regex regex = new Regex("(.*?) (.*?) ?'(.*?)'");

		Match match = regex.Match(queryString);

		if (!match.Success) {
			throw new CalabashException("Failed to parse query \"" + queryString + "\"");
		}

		var viewType = match.Groups[1].Value;
		var queryVerb = match.Groups[2].Value;
		var queryParameter = match.Groups[3].Value;

		FileLog.Log("PARSED QUERY: \"" + queryString + "\"");
		FileLog.Log(viewType);
		FileLog.Log(queryVerb);
		FileLog.Log(queryParameter);

		var canvas = UnityEngine.Object.FindObjectOfType<UnityEngine.Canvas>();

		if (viewType == "view") {
			FileLog.Log("CHECKING VIEWS");

			var views = canvas.GetComponentsInChildren<CanvasRenderer>();

			foreach (var view in views) {
				FileLog.Log("CHECKING VIEW \"" + view.gameObject.name + "\"");
				var textComponent = view.GetComponent<UnityEngine.UI.Text>();
				if (textComponent != null) {
					var viewText = textComponent.text;

					// Unity labels often have nasty newline in the end
					if (viewText.EndsWith("\n")) {
						viewText = viewText.Substring(0, viewText.Length - 1);
					}

					if (viewText == queryParameter) {
						FileLog.Log("ITS THE ONE");
						results.Add(new CalabashMatchedObject {
							label = viewText,
							rect = GetUIRect(view.gameObject, canvas)
						});
					}
				}
			}
		} else if (viewType == "button") {
			var buttons = canvas.GetComponentsInChildren<UnityEngine.UI.Button>();

			foreach (var button in buttons) {
				FileLog.Log("CHECKING BUTTON");
				var buttonText = button.GetComponentInChildren<UnityEngine.UI.Text>().text;

				// Unity labels often have nasty newline in the end
				if (buttonText.EndsWith("\n")) {
					buttonText = buttonText.Substring(0, buttonText.Length - 1);
				}

				FileLog.Log("\"" + buttonText + "\" - " + buttonText.Length.ToString());
				FileLog.Log("\"" + queryParameter + "\" - " + queryParameter.Length.ToString());

				if (buttonText == queryParameter) {
					FileLog.Log("ITS THE ONE");
					results.Add(new CalabashMatchedObject {
						label = buttonText,
						rect = GetUIRect(button.gameObject, canvas)
					});
				}
			}
		} else {
			throw new CalabashException("Unknown view type \"" + viewType + "\" in query \"" + queryString + "\"");
		}

		return results;
	}
}
