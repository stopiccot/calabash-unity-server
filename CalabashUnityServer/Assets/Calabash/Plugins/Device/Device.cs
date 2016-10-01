using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace Calabash.iOS {
	
public class Device {

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport ("__Internal")]
	private static extern int CalabashDeviceGeneration();
#endif

	public static UnityEngine.iOS.DeviceGeneration generation {
		get {
			#if UNITY_IOS && !UNITY_EDITOR
			int deviceGeneration = CalabashDeviceGeneration();
			return (UnityEngine.iOS.DeviceGeneration)deviceGeneration;
			#endif

			return UnityEngine.iOS.DeviceGeneration.Unknown;
		}
	}
}
}
