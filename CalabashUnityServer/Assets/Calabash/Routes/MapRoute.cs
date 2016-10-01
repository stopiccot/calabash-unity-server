using System;
using System.IO;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;

namespace Calabash
{
	public class MapRequestResponse {
		public string outcome;
		public string reason;
		public string status_bar_orientation;
		public List<CalabashMatchedObject> results;
	}

	public class MapRequestOrientationResponse {
		public string outcome;
		public string reason;
		public string status_bar_orientation;
		public List<string> results;
	}

	public class MapRoute : Route {
		
		[Serializable]
		public struct MapRouteOperation {
			public string method_name;
			public List<string> arguments;
		}

		[Serializable]
		public struct MapRouteRequest {
			public string query;
			public MapRouteOperation operation;
		}

		public override string HandleRequest(System.Net.HttpListenerRequest httpRequest) {
	      /*QUERY: --->
	
	      {
				:method=>:post, 
				:path=>"map", 
				:uri=>#<URI::HTTP http://127.0.0.1:37265/map>, 
				:body=>"{\"query\":\"view text:'My media'\",\"operation\":{\"method_name\":\"query\",\"arguments\":[]}}"
		    }
	
			{
				"status_bar_orientation": "down",
				"results": [{
					"id": null,
					"visible": 1,
					"label": "My media",
					"frame": {
						"y": 0,
						"width": 79.5,
						"x": 0,
						"height": 21
					},
					"enabled": true,
					"description": "<UILabel: 0x7f83b820; frame = (0 0; 79.5 21); text = 'My media'; userInteractionEnabled = NO; layer = <_UILabelLayer: 0x7f83b930>>",
					"text": "My media",
					"alpha": 1,
					"value": "My media",
					"rect": {
						"y": 339,
						"width": 79.5,
						"center_x": 65.75,
						"x": 26,
						"center_y": 349.5,
						"height": 21
					},
					"accessibilityElement": true,
					"class": "UILabel"
				}],
				"outcome": "SUCCESS"
			}*/

			var request = DeserializeRequestBody<MapRouteRequest>(httpRequest);
			if (request.operation.method_name == "orientation") {
				var t1 = CalabashServer.Instance.ExecuteOnMainThread<string>(() => {
					var orientation = Screen.orientation;
					if (orientation == ScreenOrientation.Portrait) {
						return "down";
					} else if (orientation == ScreenOrientation.PortraitUpsideDown) {
						return "up";
					} else if (orientation == ScreenOrientation.LandscapeLeft) {
						return "left";
					} else if (orientation == ScreenOrientation.LandscapeRight) {
						return "right";
					}

					throw new CalabashException("Unsupported screen orientation \"" + orientation.ToString() + "\"");
				});

				t1.Wait();

				var rr = new MapRequestOrientationResponse {
					outcome = "SUCCESS",
					status_bar_orientation = t1.Result,
					results = new List<string>() { t1.Result }
				};

				return JsonUtility.ToJson(rr);
			}

			string queryString = request.query;
//			queryString = "button marked: 'OK'";

			string failureReason = null;

			var task = CalabashServer.Instance.ExecuteOnMainThread<List<CalabashMatchedObject>>(() => {
				try {
					return CalabashServer.Instance.Query(queryString);
				} catch (CalabashException e) {
					failureReason = e.Message;
					return null;
				} catch (Exception e) {
					FileLog.Log("EXCEPTION: " + e.ToString());
					failureReason = "Horrible exception during CalabashCanvas.Instance.Query";
					return null;
				}
			});

			task.Wait();

			var matchedObjects = task.Result;

			var responseObject = new MapRequestResponse {
				outcome = "SUCCESS",
				results = new List<CalabashMatchedObject>()
			};

			if (matchedObjects != null) {
				responseObject.results = matchedObjects;
			} else {
				responseObject.outcome = "FAILURE";
				responseObject.reason = !String.IsNullOrEmpty(failureReason) ? failureReason : "Unknown reason";
			}

			return JsonUtility.ToJson(responseObject);
		}
	}
}

