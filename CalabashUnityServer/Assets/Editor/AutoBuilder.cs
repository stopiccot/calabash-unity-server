using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class AutoBuilder {

    [MenuItem("File/AutoBuilder/iOS")]
    static void PerformiOSBuild()
    {
		PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
        BuildPipeline.BuildPlayer(new EditorBuildSettingsScene[] { 
            new EditorBuildSettingsScene("Assets/Scene1/Scene2.unity", true),
		}, "Builds/iOS", BuildTarget.iOS, BuildOptions.SymlinkLibraries);
    }

	[MenuItem("File/AutoBuilder/iOSSimulator")]
	static void PerformiOSBuildSimulator()
	{
		PlayerSettings.iOS.sdkVersion = iOSSdkVersion.SimulatorSDK;
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, "CALABASH_UNITY");
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
		BuildPipeline.BuildPlayer(new EditorBuildSettingsScene[] {
			new EditorBuildSettingsScene("Assets/Scene1/Scene2.unity", true),
		}, "Builds/iOS", BuildTarget.iOS, BuildOptions.SymlinkLibraries);
	}

    [MenuItem("File/AutoBuilder/Android")]
    static void PerformAndroidBuild()
    {
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
        BuildPipeline.BuildPlayer(new EditorBuildSettingsScene[] { 
            new EditorBuildSettingsScene("Assets/Scene1/Scene1.unity", true),
		}, "Builds/Android", BuildTarget.Android, BuildOptions.None);
    }
}