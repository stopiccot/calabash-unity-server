using System;

namespace Calabash
{
	public class VersionRoute : Route {
		public override string HandleRequest(System.Net.HttpListenerRequest httpRequest) {
			/* {
				"outcome": "SUCCESS",
				"ios_version": "9.3",
				"app_id": "com.example.someapp",
				"app_name": "Some Application",
				"app_base_sdk": "iphonesimulator10.0",
				"git": {
					"remote_origin": "git@github.com:calabash\\/calabash-ios-server.git",
					"branch": "master",
					"revision": "3427493"
				},
				"iOS_version": "9.3",
				"simulator_device": "iPhone",
				"version": "0.20.0",
				"server_port": 37265,
				"model_identifier": "iPhone7,2",
				"form_factor": "iphone 6",
				"app_version": "3.10.3",
				"simulator": "CoreSimulator 303.8 - Device: iPhone 6 - Runtime: iOS 9.3 (13E233) - DeviceType: iPhone 6",
				"4inch": false,
				"iphone_app_emulated_on_ipad": false,
				"device_family": "iPhone",
				"screen_dimensions": {
					"scale": 2,
					"width": 750,
					"native_scale": 2,
					"sample": 1,
					"height": 1334
				},
				"short_version_string": "3.10",
				"system": "x86_64",
				"device_name": "iPhone Simulator"
			} */

			return "{ \"outcome\": \"SUCCESS\", \"version\": \"0.20.0\" }";
		}
	}
}

